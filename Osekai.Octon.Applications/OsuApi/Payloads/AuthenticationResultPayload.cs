using System.Text.Json.Serialization;

namespace Osekai.Octon.Applications.OsuApi.Payloads;

#nullable disable

public class AuthenticationResultPayload
{
    [JsonPropertyName("access_token")] 
    public string Token { set; get; }

    [JsonPropertyName("refresh_token")] 
    public string RefreshToken { set; get; } 
    
    [JsonPropertyName("expires_in")]
    public long ExpiresIn { set; get; }
}