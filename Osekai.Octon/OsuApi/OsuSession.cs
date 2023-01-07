namespace Osekai.Octon.OsuApi;

public class OsuSessionContainer: IOsuApiV2SessionProvider
{
    private readonly IOsuApiV2TokenUpdater _tokenUpdater;
    
    public OsuSessionContainer(int userId, string accessToken, string refreshToken, DateTimeOffset expiresAt, IOsuApiV2TokenUpdater tokenUpdater)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
        ExpiresAt = expiresAt;
        UserId = userId;
        
        _tokenUpdater = tokenUpdater;
    }
    
    public string AccessToken { get; private set; }
    public string RefreshToken { get; private set; }

    public int UserId { get; private set; }
    
    public DateTimeOffset ExpiresAt { get; private set; }
    
    public async Task<string?> GetOsuApiV2TokenAsync(CancellationToken cancellationToken = default)
    {
        if (DateTimeOffset.Now > ExpiresAt)
            (AccessToken, RefreshToken, ExpiresAt) = await _tokenUpdater.UpdateAsync(RefreshToken, cancellationToken);

        return AccessToken;
    }

    public Task<int?> GetOsuApiV2UserIdAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult<int?>(UserId);
    }
}