using Osekai.Octon.Database;
using Osekai.Octon.Database.HelperTypes;
using Osekai.Octon.Database.Models;
using Osekai.Octon.Database.Repositories.Query;
using Osekai.Octon.Exceptions;
using Osekai.Octon;
using Osekai.Octon.OsuApi;
using Osekai.Octon.OsuApi.Payloads;
using Osekai.Octon.Services.Query;

namespace Osekai.Octon.Services;

public class AuthenticationService
{
    private readonly IDatabaseUnitOfWorkFactory _databaseUnitOfWorkFactory;
    private readonly IAuthenticatedOsuApiV2Interface _authenticatedOsuApiV2Interface;
    private readonly OsuApiV2Interface _osuApiV2Interface;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly CurrentSession _currentSession;
    
    public AuthenticationService(CurrentSession currentSession, 
        IAuthenticatedOsuApiV2Interface authenticatedOsuApiV2Interface, 
        IDatabaseUnitOfWorkFactory databaseUnitOfWorkFactory,
        ITokenGenerator tokenGenerator,
        OsuApiV2Interface osuApiV2Interface)
    {
        _currentSession = currentSession;
        _databaseUnitOfWorkFactory = databaseUnitOfWorkFactory;
        _authenticatedOsuApiV2Interface = authenticatedOsuApiV2Interface;
        _osuApiV2Interface = osuApiV2Interface; 
        _tokenGenerator = tokenGenerator;
    }
    
    public async Task<Session> LogInWithTokenAsync(AuthenticateWithTokenQuery query, CancellationToken cancellationToken = default)
    {
        if (!_currentSession.IsNull())
            throw new AlreadyAuthenticatedException();

        await using IDatabaseTransactionalUnitOfWork unitOfWork = await _databaseUnitOfWorkFactory.CreateTransactional();
        
        Session? session = await unitOfWork.SessionRepository.GetSessionFromTokenAsync(
            new GetSessionByTokenQuery(query.Token), cancellationToken);
        
        if (session == null)
            throw new InvalidSessionTokenFormatException(query.Token);
        
        _currentSession.Set(session);
        
        await unitOfWork.CommitAsync(cancellationToken);
        return session;
    }

    public async Task<Session> SignInWithCodeAsync(AuthenticationWithCodeQuery query, CancellationToken cancellationToken = default)
    {
        if (!_currentSession.IsNull())
            throw new AlreadyAuthenticatedException();
        
        (OsuAuthenticationResultPayload payload, OsuUser user) = 
            await _osuApiV2Interface.AuthenticateWithCodeAsync(query.Code, cancellationToken);

        if (user.IsDeleted)
            throw new OsuUserDeletedException(user.Id);

        if (user.IsDeleted)
            throw new OsuUserRestrictedException(user.Id);
        
        await using IDatabaseTransactionalUnitOfWork unitOfWork = await _databaseUnitOfWorkFactory.CreateTransactional();

        string generatedToken;
        
        do
        {
            generatedToken = _tokenGenerator.GenerateToken();
        } while (await unitOfWork.SessionRepository.SessionExists(new SessionExistsQuery(generatedToken), cancellationToken));
        
        Session session = await unitOfWork.SessionRepository.AddOrUpdateSessionAsync(
            new AddOrUpdateSessionQuery(generatedToken, 
                new SessionPayload(payload.Token, payload.RefreshToken, DateTime.Now.AddSeconds(payload.ExpiresIn), user.Id)),
            cancellationToken);
        
        await unitOfWork.SaveAsync(cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);
        
        _currentSession.Set(session);
        
        return session;
    }
}