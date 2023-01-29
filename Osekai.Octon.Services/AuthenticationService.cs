using Microsoft.Extensions.DependencyInjection;
using Osekai.Octon.Exceptions;
using Osekai.Octon.OsuApi;
using Osekai.Octon.OsuApi.Payloads;
using Osekai.Octon.Persistence;
using Osekai.Octon.Persistence.Dtos;
using Osekai.Octon.Persistence.HelperTypes;

namespace Osekai.Octon.Services;

public class AuthenticationService
{
    protected IUnitOfWork UnitOfWork { get; }
    protected OsuApiV2Interface OsuApiV2Interface { get; }
    protected ITokenGenerator TokenGenerator { get; }
    protected IServiceScopeFactory ServiceScopeFactory { get; }
    protected CachedAuthenticatedOsuApiV2Interface AuthenticatedOsuApiV2Interface { get; }
    
    public AuthenticationService(
        IServiceScopeFactory serviceScopeFactory,
        IUnitOfWork unitOfWork,
        ITokenGenerator tokenGenerator,
        CachedAuthenticatedOsuApiV2Interface authenticatedOsuApiV2Interface,
        OsuApiV2Interface osuApiV2Interface)
    {
        ServiceScopeFactory = serviceScopeFactory;
        UnitOfWork = unitOfWork;
        OsuApiV2Interface = osuApiV2Interface; 
        TokenGenerator = tokenGenerator;
        AuthenticatedOsuApiV2Interface = authenticatedOsuApiV2Interface;
    }

    private sealed class LocalOsuApiV2TokenUpdater : IOsuApiV2TokenUpdater
    {
        private readonly OsuApiV2Interface _osuApiV2Interface;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly string _token;
        
        public LocalOsuApiV2TokenUpdater(IServiceScopeFactory unitOfWork, OsuApiV2Interface osuApiV2Interface, string token)
        {
            _token = token;
            _osuApiV2Interface = osuApiV2Interface;
            _serviceScopeFactory = unitOfWork;
        }
        
        public async Task<(string NewAccessToken, string NewRefreshToken, DateTimeOffset ExpiresAt)> UpdateAsync(string refreshToken, CancellationToken cancellationToken = default)
        {
            await using AsyncServiceScope scope = _serviceScopeFactory.CreateAsyncScope();

            IUnitOfWork unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            
            var (authenticationResultPayload, user, responseTime) = await _osuApiV2Interface.RefreshTokenAsync(refreshToken, cancellationToken);
            DateTimeOffset expiresAt = responseTime.AddSeconds(authenticationResultPayload.ExpiresIn);
            
            await unitOfWork.SessionRepository.UpdateSessionPayloadAsync(_token, 
                new SessionDtoPayload(authenticationResultPayload.Token, authenticationResultPayload.RefreshToken, expiresAt.UtcDateTime, user.Id), 
                cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);
            
            return (authenticationResultPayload.Token, authenticationResultPayload.RefreshToken, expiresAt);
        }
    }
    
    
    public readonly struct LogInWithCodeResult
    {
        public LogInWithCodeResult(OsuSessionContainer osuSessionContainer)
        {
            OsuSessionContainer = osuSessionContainer;
        }

        public OsuSessionContainer OsuSessionContainer { get; }
    }
    
    public async Task<LogInWithCodeResult> LogInWithTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        SessionDto? session = await UnitOfWork.SessionRepository.GetSessionFromTokenAsync(token, cancellationToken);
        
        if (session == null)
            throw new InvalidSessionTokenException(token);

        var osuSessionContainer = new OsuSessionContainer(
             session.Payload.OsuUserId, session.Payload.OsuApiV2Token, 
             session.Payload.OsuApiV2RefreshToken, 
             session.Payload.ExpiresAt,
             new LocalOsuApiV2TokenUpdater(ServiceScopeFactory, OsuApiV2Interface, session.Token));

        OsuUser user = await AuthenticatedOsuApiV2Interface.MeAsync(osuSessionContainer, cancellationToken: cancellationToken);

        if (user.IsDeleted)
            throw new OsuUserDeletedException(user.Id);

        if (user.IsRestricted)
            throw new OsuUserRestrictedException(user.Id);
        
        return new LogInWithCodeResult(osuSessionContainer);
    }

    public readonly struct SignInWithCodeResult
    {
        public SignInWithCodeResult(OsuSessionContainer osuSessionContainer, string token, DateTimeOffset expiresAt)
        {
            OsuSessionContainer = osuSessionContainer;
            Token = token;
            ExpiresAt = expiresAt;
        }

        public OsuSessionContainer OsuSessionContainer { get; }
        public string Token { get; }
        public DateTimeOffset ExpiresAt { get; }
    }
    
    public async Task<SignInWithCodeResult> SignInWithCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        (OsuAuthenticationResultPayload payload, OsuUser user, DateTimeOffset responseDateTime) = await OsuApiV2Interface.AuthenticateWithCodeAsync(code, cancellationToken);

        if (user.IsDeleted)
            throw new OsuUserDeletedException(user.Id);

        if (user.IsDeleted)
            throw new OsuUserRestrictedException(user.Id);
        
        string generatedToken;
        
        do generatedToken = TokenGenerator.GenerateToken();
        while (await UnitOfWork.SessionRepository.SessionExists(generatedToken, cancellationToken));

        SessionDto session = await UnitOfWork.SessionRepository.AddSessionAsync(
            generatedToken,
            new SessionDtoPayload(payload.Token, payload.RefreshToken,
                responseDateTime.AddSeconds(payload.ExpiresIn).UtcDateTime, user.Id),
            DateTimeOffset.Now.AddSeconds(Specifications.SessionTokenMaxAgeInSeconds), cancellationToken);
        
        return new SignInWithCodeResult(
            new OsuSessionContainer(
                user.Id, session.Payload.OsuApiV2Token, session.Payload.OsuApiV2RefreshToken, session.Payload.ExpiresAt, 
                new LocalOsuApiV2TokenUpdater(ServiceScopeFactory, OsuApiV2Interface, session.Token)), 
            session.Token,
            session.ExpiresAt);
    }

    public Task RevokeTokenAsync(string token, CancellationToken cancellationToken = default) =>
        UnitOfWork.SessionRepository.DeleteSessionAsync(token);
}