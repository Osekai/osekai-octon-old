using System.Text.Json;
using Microsoft.Extensions.Logging;
using Osekai.Octon.Database;
using Osekai.Octon.Database.HelperTypes;
using Osekai.Octon.Database.Models;
using Osekai.Octon.Database.Repositories.Query;
using Osekai.Octon.Exceptions;
using Osekai.Octon.OsuApi;
using Osekai.Octon.OsuApi.Payloads;

namespace Osekai.Octon;

public class CurrentSession: IOsuApiV2TokenProvider
{
    private (Session Session, SessionPayload SessionPayload)? _sessionAndPayload;

    private readonly OsuApiV2Interface _osuApiV2Interface;
    private readonly IDatabaseUnitOfWorkFactory _databaseUnitOfWorkFactory;

    private readonly ILogger<CurrentSession> _logger;

    public CurrentSession(
        OsuApiV2Interface osuApiV2Interface, 
        IDatabaseUnitOfWorkFactory databaseUnitOfWork,
        ILogger<CurrentSession> logger)
    {
        _osuApiV2Interface = osuApiV2Interface;
        _databaseUnitOfWorkFactory = databaseUnitOfWork;
        _logger = logger;
    }
    
    public void Set(Session session)
    {
        _sessionAndPayload = (session, 
            JsonSerializer.Deserialize<SessionPayload>(session.Payload) 
            ?? throw new ArgumentException("Invalid session payload"));
    }

    private async Task UpdateIfNeededAsync(CancellationToken cancellationToken)
    {
        if (_sessionAndPayload == null)
            return;
        
        Session session = _sessionAndPayload!.Value.Session;
        SessionPayload sessionPayload = _sessionAndPayload.Value.SessionPayload;
        
        IDatabaseUnitOfWork unitOfWork = await _databaseUnitOfWorkFactory.Create();
        
        try
        {
            if (DateTime.Now > session.ExpiresAt)
                throw new InvalidSessionTokenException(session.Token);

            if (DateTime.Now > sessionPayload.ExpiresAt)
            {
                (OsuAuthenticationResultPayload payload, OsuUser user) =
                    await _osuApiV2Interface.RefreshTokenAsync(
                        sessionPayload.OsuApiV2RefreshToken,
                        cancellationToken);

                if (user.IsRestricted)
                    throw new OsuUserRestrictedException(user.Id);
                
                if (user.IsDeleted)
                    throw new OsuUserDeletedException(user.Id);
                
                sessionPayload.OsuApiV2RefreshToken = payload.RefreshToken;
                sessionPayload.OsuApiV2Token = payload.Token;
                sessionPayload.ExpiresAt = DateTime.Now.AddSeconds(payload.ExpiresIn);

                session = await unitOfWork.SessionRepository.AddOrUpdateSessionAsync(
                    new AddOrUpdateSessionQuery(session.Token, sessionPayload),
                    cancellationToken);
            }
            else
                session.ExpiresAt = (await unitOfWork.SessionRepository.RefreshSessionAsync(
                    new RefreshSessionQuery(session.Token), cancellationToken))!.Value;
            
            await unitOfWork.SaveAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            unitOfWork.DiscardChanges();
            
            await unitOfWork.SessionRepository.DeleteSessionAsync(new DeleteSessionQuery(session.Token),
                cancellationToken);
            
            await unitOfWork.SaveAsync(cancellationToken);
            
            _sessionAndPayload = null;
            _logger.LogError(exception, exception.Message);
            
            throw;
        }
    }
    
    public async Task<Session?> GetAsync(CancellationToken cancellationToken = default)
    {
        if (_sessionAndPayload == null)
            return null;

        await UpdateIfNeededAsync(cancellationToken);

        return (Session)_sessionAndPayload!.Value.Session.Clone();
    }

    public async Task<SessionPayload?> GetPayloadAsync(CancellationToken cancellationToken = default)
    {
        if (_sessionAndPayload == null)
            return null;

        await UpdateIfNeededAsync(cancellationToken);

        return (SessionPayload)_sessionAndPayload!.Value.SessionPayload.Clone();
    }

    public async Task<bool> TryUpdateAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await UpdateIfNeededAsync(cancellationToken);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool IsNull() => _sessionAndPayload == null;
    
    public async Task<string?> GetOsuApiV2TokenAsync(CancellationToken cancellationToken = default)
    {
        await UpdateIfNeededAsync(cancellationToken);
        return _sessionAndPayload?.SessionPayload.OsuApiV2Token;
    }

    public int? OsuUserId => _sessionAndPayload?.SessionPayload.OsuUserId;
}