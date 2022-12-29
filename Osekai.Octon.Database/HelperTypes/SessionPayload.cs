namespace Osekai.Octon.Database.HelperTypes;

public class SessionPayload
{
    public SessionPayload(string osuApiV2Token, string osuApiV2RefreshToken, DateTime expiresAt)
    {
        OsuApiV2Token = osuApiV2Token;
        OsuApiV2RefreshToken = osuApiV2RefreshToken;
        ExpiresAt = expiresAt;
    }
    
    public string OsuApiV2Token { get; set; }
    public string OsuApiV2RefreshToken { get; set; }
    
    public DateTimeOffset ExpiresAt { get; set; }
}