using System.Diagnostics;
using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using Osekai.Octon.Options;
using Osekai.Octon.OsuApi.Payloads;

namespace Osekai.Octon.OsuApi;

public class OsuApiV2Interface
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly OsuOAuthConfiguration _osuOAuthConfiguration;
    
    public OsuApiV2Interface(IHttpClientFactory httpClientFactory, IOptions<OsuOAuthConfiguration> osuOAuthConfiguration)
    {
        _httpClientFactory = httpClientFactory;
        _osuOAuthConfiguration = osuOAuthConfiguration.Value;
    }

    public async Task<(OsuAuthenticationResultPayload, OsuUser, DateTimeOffset)> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        using HttpClient client = _httpClientFactory.CreateClient();

        HttpResponseMessage response = await client.PostAsync(
            "https://osu.ppy.sh/oauth/token", 
            JsonContent.Create(
                new OsuRefreshTokenAuthenticationPayload(
                    _osuOAuthConfiguration.ClientId, _osuOAuthConfiguration.ClientSecret, 
                    refreshToken, _osuOAuthConfiguration.RedirectUri)), 
            cancellationToken);
        
        response.EnsureSuccessStatusCode();

        OsuAuthenticationResultPayload authenticatedPayload =
            await response.Content.ReadFromJsonAsync<OsuAuthenticationResultPayload>(cancellationToken: cancellationToken) ??
            throw new InvalidDataException();
        
        DateTimeOffset responseDateTime = response.Headers.Date ?? DateTimeOffset.Now;
            
        client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Bearer {authenticatedPayload.Token}");

        response = await client.GetAsync($"https://osu.ppy.sh/api/v2/me", cancellationToken);
        
        response.EnsureSuccessStatusCode();
        OsuUser osuUser = (await response.Content.ReadFromJsonAsync<OsuUser>()) ?? throw new InvalidDataException();
        
        return (authenticatedPayload, osuUser, responseDateTime);
    }
    
    
    public async Task<(OsuAuthenticationResultPayload, OsuUser, DateTimeOffset)> AuthenticateWithCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        using HttpClient client = _httpClientFactory.CreateClient();
        
        HttpResponseMessage response = await client.PostAsync(
            "https://osu.ppy.sh/oauth/token", 
            JsonContent.Create(
                new OsuAuthenticationPayload(
                    _osuOAuthConfiguration.ClientId, _osuOAuthConfiguration.ClientSecret, 
                    code, "authorization_code", _osuOAuthConfiguration.RedirectUri)), 
            cancellationToken);

        response.EnsureSuccessStatusCode();
        
        OsuAuthenticationResultPayload authenticatedPayload = await response.Content.ReadFromJsonAsync<OsuAuthenticationResultPayload>(cancellationToken: cancellationToken)
            ?? throw new InvalidDataException();

        DateTimeOffset responseDateTime = response.Headers.Date ?? DateTimeOffset.Now;
            
        client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Bearer {authenticatedPayload.Token}");

        response = await client.GetAsync($"https://osu.ppy.sh/api/v2/me", cancellationToken);
        response.EnsureSuccessStatusCode();
        OsuUser osuUser = (await response.Content.ReadFromJsonAsync<OsuUser>()) ?? throw new InvalidDataException();

        return (authenticatedPayload, osuUser, responseDateTime);
    }
}