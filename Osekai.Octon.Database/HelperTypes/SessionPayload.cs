namespace Osekai.Octon.Database.HelperTypes;

public class SessionPayload
{
    public SessionPayload(string osuApiV2Token, string osuApiV2RefreshToken)
    {
        OsuApiV2Token = osuApiV2Token;
        OsuApiV2RefreshToken = osuApiV2RefreshToken;
    }
    
    public string OsuApiV2Token { get; }
    public string OsuApiV2RefreshToken { get; }
}