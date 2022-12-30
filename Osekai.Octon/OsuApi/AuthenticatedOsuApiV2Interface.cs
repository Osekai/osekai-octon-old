using System.Net;
using System.Net.Http.Json;
using Osekai.Octon.Exceptions;
using Osekai.Octon.OsuApi.Payloads;

namespace Osekai.Octon.OsuApi;

public class AuthenticatedOsuApiV2Interface : IAuthenticatedOsuApiV2Interface
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly CurrentSession _currentSession;

    public AuthenticatedOsuApiV2Interface(
        CurrentSession currentSession, 
        IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        _currentSession = currentSession;
    }

    private async Task<HttpClient> CreateAuthenticatedClientAsync(CancellationToken cancellationToken = default)
    {
        string token = await _currentSession.GetOsuApiV2TokenAsync(cancellationToken) ?? throw new NotAuthenticatedException();
        
        HttpClient client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Bearer {token}");
        
        return client;
    }

    public async Task<OsuUser?> SearchUserAsync(string searchString, string mode = "osu", CancellationToken cancellationToken = default)
    { 
        HttpClient client = await CreateAuthenticatedClientAsync(cancellationToken);
        HttpResponseMessage response =
            await client.GetAsync($"https://osu.ppy.sh/api/v2/users/{searchString}/{mode}", cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<OsuUser>(cancellationToken: cancellationToken);
    }
    
    public async Task<OsuUser> MeAsync(string mode = "osu", CancellationToken cancellationToken = default)
    {
        HttpClient client = await CreateAuthenticatedClientAsync(cancellationToken);
        int osuUserId = _currentSession.OsuUserId!.Value;

        HttpResponseMessage response =
            await client.GetAsync($"https://osu.ppy.sh/api/v2/users/{osuUserId}/{mode}", cancellationToken);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<OsuUser>(cancellationToken: cancellationToken) ?? throw new InvalidDataException();
    }
}