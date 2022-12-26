using System.Text.Json;
using Microsoft.Extensions.Logging;
using Osekai.Octon.Applications.OsuApiV2;
using Osekai.Octon.Applications.OsuApiV2.Payloads;
using Osekai.Octon.Database;
using Osekai.Octon.Database.HelperTypes;
using Osekai.Octon.Database.Models;
using Osekai.Octon.Database.Repositories.Query;
using Osekai.Octon.Exceptions;

namespace Osekai.Octon.Applications;

public class CurrentSession: IOsuApiV2TokenProvider
{
    private (Session Session, SessionPayload SessionPayload)? _sessionAndPayload;

    private readonly OsuApiV2.OsuApiV2 _osuApiV2;
    private readonly IUnitOfWork _unitOfWork;

    private readonly ILogger<CurrentSession> _logger;

    public CurrentSession(
        OsuApiV2.OsuApiV2 osuApiV2, 
        IUnitOfWork unitOfWork,
        ILogger<CurrentSession> logger)
    {
        _osuApiV2 = osuApiV2;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    
    public void Set(Session session)
    {
        if (_sessionAndPayload != null)
            throw new InvalidOperationException("Session is already set");

        _sessionAndPayload = (session, 
            JsonSerializer.Deserialize<SessionPayload>(session.Payload) 
            ?? throw new ArgumentException("Invalid session payload"));
    }

    private async Task UpdateIfNeededAsync(CancellationToken cancellationToken)
    {
        Session session = _sessionAndPayload!.Value.Session;
        SessionPayload sessionPayload = _sessionAndPayload.Value.SessionPayload;

        try
        {
            if (DateTime.Now > session.ExpiresAt)
                throw new InvalidSessionTokenException(session.Token);

            if (DateTime.Now > sessionPayload.ExpiresAt)
            {
                AuthenticationResultPayload payload =
                    await _osuApiV2.AuthenticateWithRefreshTokenAsync(
                        sessionPayload.OsuApiV2RefreshToken,
                        cancellationToken);

                sessionPayload.OsuApiV2RefreshToken = payload.RefreshToken;
                sessionPayload.OsuApiV2Token = payload.Token;
                sessionPayload.ExpiresAt = DateTime.Now.AddSeconds(payload.ExpiresIn);

                await _unitOfWork.SessionRepository.AddOrUpdateSessionAsync(
                    new AddOrReplaceSessionQuery(session.Token, sessionPayload),
                    cancellationToken);

                await _unitOfWork.SaveAsync(cancellationToken);
            }
        }
        catch (Exception exception)
        {
            _unitOfWork.DiscardChanges();

            await _unitOfWork.SessionRepository.DeleteTokenAsync(new DeleteTokenQuery(session.Token),
                cancellationToken);
            
            await _unitOfWork.SaveAsync(cancellationToken);
            
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

        return _sessionAndPayload!.Value.Session;
    }

    public async Task<SessionPayload?> GetPayloadAsync(CancellationToken cancellationToken = default)
    {
        if (_sessionAndPayload == null)
            return null;

        await UpdateIfNeededAsync(cancellationToken);

        return _sessionAndPayload!.Value.SessionPayload;
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
        return (await GetPayloadAsync(cancellationToken))?.OsuApiV2Token;
    }
}