using Osekai.Octon.Database;
using Osekai.Octon.Database.HelperTypes;
using Osekai.Octon.Exceptions;
using Osekai.Octon.Database.Dtos;
using Osekai.Octon.OsuApi;
using Osekai.Octon.OsuApi.Payloads;

namespace Osekai.Octon.Services;

public class AuthenticationService
{
    private readonly IDatabaseUnitOfWorkFactory _databaseUnitOfWorkFactory;
    private readonly OsuApiV2Interface _osuApiV2Interface;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly CachedAuthenticatedOsuApiV2Interface _authenticatedOsuApiV2Interface;
    
    public AuthenticationService(
        IDatabaseUnitOfWorkFactory databaseUnitOfWorkFactory,
        ITokenGenerator tokenGenerator,
        CachedAuthenticatedOsuApiV2Interface authenticatedOsuApiV2Interface,
        OsuApiV2Interface osuApiV2Interface)
    {
        _databaseUnitOfWorkFactory = databaseUnitOfWorkFactory;
        _osuApiV2Interface = osuApiV2Interface; 
        _tokenGenerator = tokenGenerator;
        _authenticatedOsuApiV2Interface = authenticatedOsuApiV2Interface;
    }

    public class LocalOsuApiV2TokenUpdate : IOsuApiV2TokenUpdater
    {
        private readonly OsuApiV2Interface _osuApiV2Interface;
        private readonly IDatabaseUnitOfWorkFactory _databaseUnitOfWorkFactory;
        
        public LocalOsuApiV2TokenUpdate(IDatabaseUnitOfWorkFactory databaseUnitOfWorkFactory, OsuApiV2Interface osuApiV2Interface)
        {
            _osuApiV2Interface = osuApiV2Interface;
            _databaseUnitOfWorkFactory = databaseUnitOfWorkFactory;
        }
        
        public async Task<(string NewAccessToken, string NewRefreshToken, DateTimeOffset ExpiresAt)> UpdateAsync(string refreshToken, CancellationToken cancellationToken = default)
        {
            await using IDatabaseUnitOfWork databaseUnitOfWork = await _databaseUnitOfWorkFactory.CreateAsync(cancellationToken);
            
            var (authenticationResultPayload, user, responseTime) = await _osuApiV2Interface.RefreshTokenAsync(refreshToken, cancellationToken);
            DateTimeOffset expiresAt = responseTime.AddSeconds(authenticationResultPayload.ExpiresIn);
            
            await databaseUnitOfWork.SessionRepository.UpdateSessionPayloadAsync(refreshToken, 
                new SessionDtoPayload(authenticationResultPayload.Token, authenticationResultPayload.RefreshToken, expiresAt.UtcDateTime, user.Id), 
                cancellationToken);
            
            return (authenticationResultPayload.Token, authenticationResultPayload.RefreshToken, expiresAt);
        }
    }
    
    public async Task<OsuSessionContainer> LogInWithTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        SessionDto? session;
        await using (IDatabaseUnitOfWork unitOfWork = await _databaseUnitOfWorkFactory.CreateAsync(cancellationToken))
            session = await unitOfWork.SessionRepository.GetSessionFromTokenAsync(token, cancellationToken);

        if (session == null)
            throw new InvalidSessionTokenException(token);

        OsuSessionContainer osuSessionContainer;
        try
        {
             osuSessionContainer = new OsuSessionContainer(
                session.Payload.OsuUserId, session.Payload.OsuApiV2Token, 
                session.Payload.OsuApiV2RefreshToken, 
                session.Payload.ExpiresAt,
                new LocalOsuApiV2TokenUpdate(_databaseUnitOfWorkFactory, _osuApiV2Interface));

            OsuUser user = await _authenticatedOsuApiV2Interface.MeAsync(osuSessionContainer, cancellationToken: cancellationToken);

            if (user.IsDeleted)
                throw new OsuUserDeletedException(user.Id);

            if (user.IsRestricted)
                throw new OsuUserRestrictedException(user.Id);
        }
        catch
        {
            await using IDatabaseUnitOfWork recoveryUnitOfWork = await _databaseUnitOfWorkFactory.CreateAsync(cancellationToken);
            await recoveryUnitOfWork.SessionRepository.DeleteSessionAsync(token, cancellationToken);

            throw;
        }

        return osuSessionContainer;
    }

    public async Task<SessionDto> SignInWithCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        (OsuAuthenticationResultPayload payload, OsuUser user, DateTimeOffset responseDateTime) = await _osuApiV2Interface.AuthenticateWithCodeAsync(code, cancellationToken);

        if (user.IsDeleted)
            throw new OsuUserDeletedException(user.Id);

        if (user.IsDeleted)
            throw new OsuUserRestrictedException(user.Id);
        
        await using IDatabaseUnitOfWork unitOfWork = await _databaseUnitOfWorkFactory.CreateAsync(cancellationToken);

        string generatedToken;
        do
        {
            generatedToken = _tokenGenerator.GenerateToken();
        } while (await unitOfWork.SessionRepository.SessionExists(generatedToken, cancellationToken));

        SessionDto session = await unitOfWork.SessionRepository.AddSessionAsync(
            generatedToken,
            new SessionDtoPayload(payload.Token, payload.RefreshToken,
                responseDateTime.AddSeconds(payload.ExpiresIn).UtcDateTime, user.Id), 
            DateTimeOffset.Now.AddSeconds(Specifications.SessionTokenMaxAgeInSeconds));
        
        await unitOfWork.SaveAsync(cancellationToken);
        
        return session;
    }
}