namespace Osekai.Octon.OsuApi;

public interface IOsuApiV2TokenUpdater
{ 
    Task<(string NewAccessToken, string NewRefreshToken, DateTimeOffset ExpiresAt)> UpdateAsync(string refreshToken, CancellationToken cancellationToken = default);
}