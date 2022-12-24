using Osekai.Octon.Database;
using Osekai.Octon.Database.HelperTypes;
using Osekai.Octon.Database.Models;
using Osekai.Octon.Database.Repositories.Query;
using Osekai.Octon.Exceptions;
using Osekai.Octon.Providers;
using Osekai.Octon.Services.Query;

namespace Osekai.Octon.Services;

public class AuthenticationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly OsuApiV2Provider _osuApiV2Provider;
    private readonly ITokenProvider _tokenProvider;
    
    public AuthenticationService(IUnitOfWork unitOfWork, ITokenProvider tokenProvider, OsuApiV2Provider osuApiV2Provider)
    {
        _unitOfWork = unitOfWork;
        _osuApiV2Provider = osuApiV2Provider;
        _tokenProvider = tokenProvider;
    }
    
    public async Task<Session> AuthenticateWithTokenAsync(AuthenticateWithTokenQuery query, CancellationToken cancellationToken = default)
    {
        Session? session = await _unitOfWork.SessionRepository.GetSessionFromTokenAsync(new GetSessionByTokenQuery(query.Token), cancellationToken);
        
        if (session == null)
            throw new InvalidSessionTokenException(query.Token);

        return session;
    }
    
    public async Task<Session> AuthenticateWithCodeAsync(AuthenticationWithCodeQuery query, CancellationToken cancellationToken = default)
    {
        OsuApiV2Provider.AuthenticatedPayload payload = await _osuApiV2Provider.AuthenticateAsync(query.Code, cancellationToken);

        string generatedToken;
        for (;;)
        {
            generatedToken = _tokenProvider.GenerateToken();
            if (!await _unitOfWork.SessionRepository.SessionExists(new SessionExistsQuery(generatedToken), cancellationToken))
                break;
        }
        
        Session session = await _unitOfWork.SessionRepository.AddOrReplaceSessionAsync(
            new AddOrReplaceSessionQuery(generatedToken, new SessionPayload(payload.Token, payload.RefreshToken)),
            cancellationToken);
        
        await _unitOfWork.SaveAsync(cancellationToken);

        return session;
    }
}