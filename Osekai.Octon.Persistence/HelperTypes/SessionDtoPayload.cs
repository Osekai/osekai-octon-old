namespace Osekai.Octon.Persistence.HelperTypes;

public class SessionDtoPayload: ICloneable
{
    public SessionDtoPayload(string osuApiV2Token, string osuApiV2RefreshToken, DateTime expiresAt, int osuUserId)
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

    public object Clone() => 
        new SessionDtoPayload(OsuApiV2Token, OsuApiV2RefreshToken, ExpiresAt, OsuUserId);
}