namespace Osekai.Octon.Domain.Entities;

public class SessionPayload
{
    public SessionPayload(string osuApiV2Token, string osuApiV2RefreshToken, DateTime expiresAt, int osuUserId)
    {
        OsuApiV2Token = osuApiV2Token;
        OsuApiV2RefreshToken = osuApiV2RefreshToken;
        ExpiresAt = expiresAt;
        OsuUserId = osuUserId;
    }
    
    public string OsuApiV2Token { get; set; }
    public string OsuApiV2RefreshToken { get; set; }
    public int OsuUserId { get; set; }
    public DateTime ExpiresAt { get; set; }
}