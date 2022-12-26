using System.Net;
using System.Net.Http.Json;
using Osekai.Octon.Applications.OsuApi.Payloads;
using Osekai.Octon.Exceptions;

namespace Osekai.Octon.Applications.OsuApi;

public class AuthenticatedOsuApiV2Interface : IAuthenticatedOsuApiV2Interface
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IOsuApiV2TokenProvider _tokenProvider;
    private readonly OsuApiTimeThrottler _timeThrottler;

    public AuthenticatedOsuApiV2Interface(OsuApiTimeThrottler timeThrottler, IOsuApiV2TokenProvider tokenProvider, IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        _tokenProvider = tokenProvider;
        _timeThrottler = timeThrottler;
    }

    private async Task<HttpClient> CreateAuthenticatedClientAsync(CancellationToken cancellationToken = default)
    {
        await _timeThrottler.WaitAsync(cancellationToken);

        string token = await _tokenProvider.GetOsuApiV2TokenAsync(cancellationToken) ?? throw new NotAuthenticatedException();
        
        HttpClient client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Bearer {token}");
        
        return client;
    }

    public async Task<User?> SearchUserAsync(string searchString, string mode = "osu", CancellationToken cancellationToken = default)
    { 
        HttpClient client = await CreateAuthenticatedClientAsync(cancellationToken);
        HttpResponseMessage response = await client.GetAsync($"https://osu.ppy.sh/api/v2/users/{searchString}/{mode}", cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;
        
        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadFromJsonAsync<User>(cancellationToken: cancellationToken);
    }
    
    public async Task<User?> MeAsync(string mode = "osu", CancellationToken cancellationToken = default)
    { 
        HttpClient client = await CreateAuthenticatedClientAsync(cancellationToken);
        HttpResponseMessage response = await client.GetAsync($"https://osu.ppy.sh/api/v2/me/{mode}", cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;
        
        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadFromJsonAsync<User>(cancellationToken: cancellationToken);
    }
}