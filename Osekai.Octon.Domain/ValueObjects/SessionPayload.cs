namespace Osekai.Octon.Domain.ValueObjects;

public readonly struct SessionPayload
{
    public SessionPayload(string osuApiV2Token, string osuApiV2RefreshToken, DateTime expiresAt, int osuUserId)
    {
        OsuApiV2Token = osuApiV2Token;
        OsuApiV2RefreshToken = osuApiV2RefreshToken;
        ExpiresAt = expiresAt;
        OsuUserId = osuUserId;
    }
    
    public string OsuApiV2Token { get; }
    public string OsuApiV2RefreshToken { get; }
    public int OsuUserId { get; }
    public DateTime ExpiresAt { get; }
}