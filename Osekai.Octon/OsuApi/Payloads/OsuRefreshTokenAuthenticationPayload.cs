using System.Text.Json.Serialization;

namespace Osekai.Octon.OsuApi.Payloads;

public class OsuRefreshTokenAuthenticationPayload
{
    public OsuRefreshTokenAuthenticationPayload(int clientId, string clientSecret, string refreshToken, string redirectUri)
    {
        ClientId = clientId;
        ClientSecret = clientSecret;
        RefreshToken = refreshToken;
        GrantType = "refresh_token";
        RedirectUri = redirectUri;
    }

    [JsonPropertyName("client_id")]
    public int ClientId { get; }
        
    [JsonPropertyName("client_secret")]
    public string ClientSecret { get; }
        
    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; }
        
    [JsonPropertyName("grant_type")]
    public string GrantType { get; }
        
    [JsonPropertyName("redirect_uri")]
    public string RedirectUri { get; }
}