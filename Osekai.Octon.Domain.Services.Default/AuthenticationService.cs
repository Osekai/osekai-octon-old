using Microsoft.Extensions.DependencyInjection;
using Osekai.Octon.Domain;
using Osekai.Octon.Domain.Repositories;
using Osekai.Octon.Domain.Services;
using Osekai.Octon.Domain.ValueObjects;
using Osekai.Octon.Exceptions;
using Osekai.Octon.OsuApi;
using Osekai.Octon.OsuApi.Payloads;
using Osekai.Octon.Persistence;
using Session = Osekai.Octon.Domain.AggregateRoots.Session;

namespace Osekai.Octon.Domain.Services.Default;

public class AuthenticationService : IAuthenticationService
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

            Session? session = (await unitOfWork.SessionRepository.GetSessionByTokenAsync(_token, cancellationToken));

            if (session == null)
                throw new InvalidOperationException("Invalid session");

            session.Payload = new SessionPayload(
                authenticationResultPayload.Token, authenticationResultPayload.RefreshToken,
                user.Id, expiresAt.UtcDateTime);
            
            await unitOfWork.SessionRepository.SaveSessionAsyncAsync(session, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            
            return (authenticationResultPayload.Token, authenticationResultPayload.RefreshToken, expiresAt);
        }
    }
    
    
    public async Task<IAuthenticationService.LogInWithCodeResult> LogInWithTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        Session? session = await UnitOfWork.SessionRepository.GetSessionByTokenAsync(token, cancellationToken);
        
        if (session == null)
            throw new InvalidSessionTokenException(token);

        var osuSessionContainer = new OsuSessionContainer(
             session.Payload!.Value.Value.OsuUserId, session.Payload!.Value.Value.OsuApiV2Token, 
             session.Payload!.Value.Value.OsuApiV2RefreshToken, 
             session.Payload!.Value.Value.ExpiresAt,
             new LocalOsuApiV2TokenUpdater(ServiceScopeFactory, OsuApiV2Interface, session.Token));

        OsuUser user = await AuthenticatedOsuApiV2Interface.MeAsync(osuSessionContainer, cancellationToken: cancellationToken);

        if (user.IsDeleted)
            throw new OsuUserDeletedException(user.Id);

        if (user.IsRestricted)
            throw new OsuUserRestrictedException(user.Id);
        
        return new IAuthenticationService.LogInWithCodeResult(osuSessionContainer);
    }


    public async Task<IAuthenticationService.SignInWithCodeResult> SignInWithCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        (OsuAuthenticationResultPayload payload, OsuUser user, DateTimeOffset responseDateTime) = await OsuApiV2Interface.AuthenticateWithCodeAsync(code, cancellationToken);

        if (user.IsDeleted)
            throw new OsuUserDeletedException(user.Id);

        if (user.IsDeleted)
            throw new OsuUserRestrictedException(user.Id);
        
        string generatedToken;
        
        do generatedToken = TokenGenerator.GenerateToken();
        while (await UnitOfWork.SessionRepository.SessionExistsByToken(generatedToken, cancellationToken));

        Session session = new Session(generatedToken,
            DateTimeOffset.Now.AddSeconds(Specifications.SessionTokenMaxAgeInSeconds))
        {
            Payload = new SessionPayload(payload.Token, payload.RefreshToken,
                user.Id, responseDateTime.AddSeconds(payload.ExpiresIn).UtcDateTime)
        };
        
        await UnitOfWork.SessionRepository.AddSessionAsync(session, cancellationToken);
        
        return new IAuthenticationService.SignInWithCodeResult(
            new OsuSessionContainer(
                user.Id, session.Payload!.Value.Value.OsuApiV2Token, session.Payload.Value.Value.OsuApiV2RefreshToken, session.Payload.Value.Value.ExpiresAt, 
                new LocalOsuApiV2TokenUpdater(ServiceScopeFactory, OsuApiV2Interface, session.Token)), 
            session.Token,
            session.ExpiresAt);
    }

    public Task RevokeTokenAsync(string token, CancellationToken cancellationToken = default) =>
        UnitOfWork.SessionRepository.DeleteSessionByTokenAsync(token);
}