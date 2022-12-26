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
    private readonly IDatabaseUnitOfWork _databaseUnitOfWork;
    private readonly OsuApiV2Interface _osuApiV2Interface;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly CurrentSession _currentSession;
    
    public AuthenticationService(CurrentSession currentSession, IDatabaseUnitOfWork databaseUnitOfWork, ITokenGenerator tokenGenerator, OsuApiV2Interface osuApiV2Interface)
    {
        _currentSession = currentSession;
        _databaseUnitOfWork = databaseUnitOfWork;
        _osuApiV2Interface = osuApiV2Interface; 
        _tokenGenerator = tokenGenerator;
    }
    
    public async Task<Session> LogInWithTokenAsync(AuthenticateWithTokenQuery query, CancellationToken cancellationToken = default)
    {
        if (!_currentSession.IsNull())
            throw new AlreadyAuthenticatedException();
        
        Session? session = await _databaseUnitOfWork.SessionRepository.GetSessionFromTokenAsync(new GetSessionByTokenQuery(query.Token), cancellationToken);
        
        if (session == null)
            throw new InvalidSessionTokenFormatException(query.Token);
        
        _currentSession.Set(session);

        return session;
    }

    public async Task<Session> SignInWithCodeAsync(AuthenticationWithCodeQuery query, CancellationToken cancellationToken = default)
    {
        if (!_currentSession.IsNull())
            throw new AlreadyAuthenticatedException();
        
        AuthenticationResultPayload payload = await _osuApiV2Interface.AuthenticateWithCodeAsync(query.Code, cancellationToken);

        string generatedToken;
        
        do
        {
            generatedToken = _tokenGenerator.GenerateToken();
        } while (await _databaseUnitOfWork.SessionRepository.SessionExists(new SessionExistsQuery(generatedToken), cancellationToken));
        
        Session session = await _databaseUnitOfWork.SessionRepository.AddOrUpdateSessionAsync(
            new AddOrUpdateSessionQuery(generatedToken, new SessionPayload(payload.Token, payload.RefreshToken, DateTime.Now.AddSeconds(payload.ExpiresIn))),
            cancellationToken);
        
        await _databaseUnitOfWork.SaveAsync(cancellationToken);
        
        _currentSession.Set(session);
        
        return session;
    }
}