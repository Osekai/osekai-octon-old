using System.Text.Json.Serialization;

namespace Osekai.Octon.Applications.OsuApiV2Payloads;


public class AuthenticationPayload
{
    public AuthenticationPayload(int clientId, string clientSecret, string code, string grantType, string redirectUri)
    {
        ClientId = clientId;
        ClientSecret = clientSecret;
        Code = code;
        GrantType = grantType;
        RedirectUri = redirectUri;
    }

    [JsonPropertyName("client_id")]
    public int ClientId { get; }
        
    [JsonPropertyName("client_secret")]
    public string ClientSecret { get; }
        
    [JsonPropertyName("code")]
    public string Code { get; }
        
    [JsonPropertyName("grant_type")]
    public string GrantType { get; }
        
    [JsonPropertyName("redirect_uri")]
    public string RedirectUri { get; }
}