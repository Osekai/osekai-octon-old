using Osekai.Octon.OsuApi;

namespace Osekai.Octon.Domain.Services;

public interface IAuthenticationService
{
    public readonly struct LogInWithCodeResult
    {
        public LogInWithCodeResult(OsuSessionContainer osuSessionContainer)
        {
            OsuSessionContainer = osuSessionContainer;
        }

        public OsuSessionContainer OsuSessionContainer { get; }
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
    
    Task<LogInWithCodeResult> LogInWithTokenAsync(string token, CancellationToken cancellationToken = default);
    Task<SignInWithCodeResult> SignInWithCodeAsync(string code, CancellationToken cancellationToken = default);
    Task RevokeTokenAsync(string token, CancellationToken cancellationToken = default);
}