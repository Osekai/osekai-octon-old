using Osekai.Octon.Database;
using Osekai.Octon.Database.HelperTypes;
using Osekai.Octon.Database.Models;
using Osekai.Octon.Database.Repositories.Query;
using Osekai.Octon.Exceptions;
using Osekai.Octon.Applications;
using Osekai.Octon.Applications.OsuApiV2;
using Osekai.Octon.Applications.OsuApiV2.Payloads;
using Osekai.Octon.Services.Query;

namespace Osekai.Octon.Services;

public class AuthenticationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly OsuApiV2 _osuApiV2;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly CurrentSession _currentSession;
    
    public AuthenticationService(CurrentSession currentSession, IUnitOfWork unitOfWork, ITokenGenerator tokenGenerator, OsuApiV2 osuApiV2)
    {
        _currentSession = currentSession;
        _unitOfWork = unitOfWork;
        _osuApiV2 = osuApiV2; 
        _tokenGenerator = tokenGenerator;
    }
    
    public async Task<Session> LogInWithTokenAsync(AuthenticateWithTokenQuery query, CancellationToken cancellationToken = default)
    {
        if (!_currentSession.IsNull())
            throw new AlreadyAuthenticatedException();
        
        Session? session = await _unitOfWork.SessionRepository.GetSessionFromTokenAsync(new GetSessionByTokenQuery(query.Token), cancellationToken);
        
        if (session == null)
            throw new InvalidSessionTokenFormatException(query.Token);
        
        _currentSession.Set(session);

        return session;
    }

    public async Task<Session> SignInWithCodeAsync(AuthenticationWithCodeQuery query, CancellationToken cancellationToken = default)
    {
        if (!_currentSession.IsNull())
            throw new AlreadyAuthenticatedException();
        
        AuthenticationResultPayload payload = await _osuApiV2.AuthenticateWithCodeAsync(query.Code, cancellationToken);

        string generatedToken;
        for (;;)
        {
            generatedToken = _tokenGenerator.GenerateToken();
            if (!await _unitOfWork.SessionRepository.SessionExists(new SessionExistsQuery(generatedToken), cancellationToken))
                break;
        }
        
        Session session = await _unitOfWork.SessionRepository.AddOrUpdateSessionAsync(
            new AddOrReplaceSessionQuery(generatedToken, new SessionPayload(payload.Token, payload.RefreshToken, DateTime.Now.AddSeconds(payload.ExpiresIn))),
            cancellationToken);
        
        await _unitOfWork.SaveAsync(cancellationToken);
        
        _currentSession.Set(session);
        
        return session;
    }
}