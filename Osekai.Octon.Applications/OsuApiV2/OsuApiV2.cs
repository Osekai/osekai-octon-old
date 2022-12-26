using System.Diagnostics;
using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using Osekai.Octon.Applications.OsuApiV2.Payloads;
using Osekai.Octon.Applications.OsuApiV2Payloads;
using Osekai.Octon.Options;

namespace Osekai.Octon.Applications.OsuApiV2;

public class OsuApiV2
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly OsuOAuthConfiguration _osuOAuthConfiguration;
    
    public OsuApiV2(IHttpClientFactory httpClientFactory, IOptions<OsuOAuthConfiguration> osuOAuthConfiguration)
    {
        _httpClientFactory = httpClientFactory;
        _osuOAuthConfiguration = osuOAuthConfiguration.Value;
    }

    public async Task<AuthenticationResultPayload> AuthenticateWithRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        using HttpClient client = _httpClientFactory.CreateClient();

        HttpResponseMessage response = await client.PostAsync(
            "https://osu.ppy.sh/oauth/token", 
            JsonContent.Create(
                new AuthenticationPayload(
                    _osuOAuthConfiguration.ClientId, _osuOAuthConfiguration.ClientSecret, 
                    refreshToken, "refresh_token", _osuOAuthConfiguration.RedirectUri)), 
            cancellationToken);
        
        response.EnsureSuccessStatusCode();
        
        AuthenticationResultPayload? authenticatedPayload = await response.Content.ReadFromJsonAsync<AuthenticationResultPayload>(cancellationToken: cancellationToken = default);
        Debug.Assert(authenticatedPayload != null);

        return authenticatedPayload!;
    }
    
    public async Task<AuthenticationResultPayload> AuthenticateWithCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        using HttpClient client = _httpClientFactory.CreateClient();

        HttpResponseMessage response = await client.PostAsync(
            "https://osu.ppy.sh/oauth/token", 
            JsonContent.Create(
                new AuthenticationPayload(
                    _osuOAuthConfiguration.ClientId, _osuOAuthConfiguration.ClientSecret, 
                    code, "authorization_code", _osuOAuthConfiguration.RedirectUri)), 
            cancellationToken);

        response.EnsureSuccessStatusCode();

        AuthenticationResultPayload? authenticatedPayload = await response.Content.ReadFromJsonAsync<AuthenticationResultPayload>(cancellationToken: cancellationToken);
        Debug.Assert(authenticatedPayload != null);

        return authenticatedPayload!;
    }
}