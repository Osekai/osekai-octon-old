using Osekai.Octon.Exceptions;
using Osekai.Octon.OsuApi;
using Osekai.Octon.OsuApi.Payloads;
using Osekai.Octon.Persistence;
using Osekai.Octon.Persistence.Dtos;
using Osekai.Octon.Persistence.HelperTypes;

namespace Osekai.Octon.Services;

public class AuthenticationService
{
    protected IDatabaseUnitOfWork UnitOfWork { get; }
    protected OsuApiV2Interface OsuApiV2Interface { get; }
    protected ITokenGenerator TokenGenerator { get; }
    protected CachedAuthenticatedOsuApiV2Interface AuthenticatedOsuApiV2Interface { get; }
    
    public AuthenticationService(
        IDatabaseUnitOfWork unitOfWork,
        ITokenGenerator tokenGenerator,
        CachedAuthenticatedOsuApiV2Interface authenticatedOsuApiV2Interface,
        OsuApiV2Interface osuApiV2Interface)
    {
        UnitOfWork = unitOfWork;
        OsuApiV2Interface = osuApiV2Interface; 
        TokenGenerator = tokenGenerator;
        AuthenticatedOsuApiV2Interface = authenticatedOsuApiV2Interface;
    }

    private sealed class LocalOsuApiV2TokenUpdate : IOsuApiV2TokenUpdater
    {
        private readonly OsuApiV2Interface _osuApiV2Interface;
        private readonly IDatabaseUnitOfWork _unitOfWork;
        
        public LocalOsuApiV2TokenUpdate(IDatabaseUnitOfWork unitOfWork, OsuApiV2Interface osuApiV2Interface)
        {
            _osuApiV2Interface = osuApiV2Interface;
            _unitOfWork = unitOfWork;
        }
        
        public async Task<(string NewAccessToken, string NewRefreshToken, DateTimeOffset ExpiresAt)> UpdateAsync(string refreshToken, CancellationToken cancellationToken = default)
        {
            var (authenticationResultPayload, user, responseTime) = await _osuApiV2Interface.RefreshTokenAsync(refreshToken, cancellationToken);
            DateTimeOffset expiresAt = responseTime.AddSeconds(authenticationResultPayload.ExpiresIn);
            
            await _unitOfWork.SessionRepository.UpdateSessionPayloadAsync(refreshToken, 
                new SessionDtoPayload(authenticationResultPayload.Token, authenticationResultPayload.RefreshToken, expiresAt.UtcDateTime, user.Id), 
                cancellationToken);

            return (authenticationResultPayload.Token, authenticationResultPayload.RefreshToken, expiresAt);
        }
    }
    
    public async Task<OsuSessionContainer> LogInWithTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        SessionDto? session = await UnitOfWork.SessionRepository.GetSessionFromTokenAsync(token);
        
        if (session == null)
            throw new InvalidSessionTokenException(token);

        var osuSessionContainer = new OsuSessionContainer(
             session.Payload.OsuUserId, session.Payload.OsuApiV2Token, 
             session.Payload.OsuApiV2RefreshToken, 
             session.Payload.ExpiresAt,
             new LocalOsuApiV2TokenUpdate(UnitOfWork, OsuApiV2Interface));

        OsuUser user = await AuthenticatedOsuApiV2Interface.MeAsync(osuSessionContainer, cancellationToken: cancellationToken);

        if (user.IsDeleted)
            throw new OsuUserDeletedException(user.Id);

        if (user.IsRestricted)
            throw new OsuUserRestrictedException(user.Id);
        
        return osuSessionContainer;
    }
    
    public async Task<(OsuSessionContainer OsuSessionContainer, string Token, DateTimeOffset ExpiresAt)> SignInWithCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        (OsuAuthenticationResultPayload payload, OsuUser user, DateTimeOffset responseDateTime) = await OsuApiV2Interface.AuthenticateWithCodeAsync(code, cancellationToken);

        if (user.IsDeleted)
            throw new OsuUserDeletedException(user.Id);

        if (user.IsDeleted)
            throw new OsuUserRestrictedException(user.Id);
        
        string generatedToken;
        
        do generatedToken = TokenGenerator.GenerateToken();
        while (await UnitOfWork.SessionRepository.SessionExists(generatedToken, cancellationToken));

        SessionDto sessionDto = await UnitOfWork.SessionRepository.AddSessionAsync(
            generatedToken,
            new SessionDtoPayload(payload.Token, payload.RefreshToken,
                responseDateTime.AddSeconds(payload.ExpiresIn).UtcDateTime, user.Id),
            DateTimeOffset.Now.AddSeconds(Specifications.SessionTokenMaxAgeInSeconds));
        
        return (
            new OsuSessionContainer(
                user.Id, sessionDto.Payload.OsuApiV2Token, sessionDto.Payload.OsuApiV2RefreshToken, sessionDto.Payload.ExpiresAt, 
                new LocalOsuApiV2TokenUpdate(UnitOfWork, OsuApiV2Interface)), 
            sessionDto.Token,
            sessionDto.ExpiresAt);
    }
}