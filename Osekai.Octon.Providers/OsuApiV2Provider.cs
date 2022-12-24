using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using Osekai.Octon.Options;

namespace Osekai.Octon.Providers;

public class OsuApiV2Provider
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly OsuOAuthConfiguration _osuOAuthConfiguration;
    
    public OsuApiV2Provider(IHttpClientFactory httpClientFactory, IOptions<OsuOAuthConfiguration> osuOAuthConfiguration)
    {
        _httpClientFactory = httpClientFactory;
        _osuOAuthConfiguration = osuOAuthConfiguration.Value;
    }

    public class AuthenticatedPayload
    {
        public AuthenticatedPayload(string token, string refreshToken)
        {
            Token = token;
            RefreshToken = refreshToken;
        }
        
        public string Token { get; }
        public string RefreshToken { get; }
    }

    class AuthenticatePayload
    {
        public AuthenticatePayload(int clientId, string clientSecret, string code, string grantType, string redirectUri)
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

    public async Task<AuthenticatedPayload> AuthenticateAsync(string code, CancellationToken cancellationToken)
    {
        using HttpClient client = _httpClientFactory.CreateClient();

        HttpResponseMessage response = await client.PostAsync(
            "https://osu.ppy.sh/oauth/token", 
            JsonContent.Create(
                new AuthenticatePayload(
                    _osuOAuthConfiguration.ClientId, _osuOAuthConfiguration.ClientSecret, 
                    code, "authorization_code", _osuOAuthConfiguration.RedirectUri)), 
            cancellationToken);

        response.EnsureSuccessStatusCode();

        AuthenticatedPayload? authenticatedPayload = await response.Content.ReadFromJsonAsync<AuthenticatedPayload>(cancellationToken: cancellationToken);
        Debug.Assert(authenticatedPayload != null);

        return authenticatedPayload!;
    }
}