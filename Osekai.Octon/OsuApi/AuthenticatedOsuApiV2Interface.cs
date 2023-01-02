using System.Net;
using System.Net.Http.Json;
using Osekai.Octon.Exceptions;
using Osekai.Octon.OsuApi.Payloads;

namespace Osekai.Octon.OsuApi;

public class AuthenticatedOsuApiV2Interface : IAuthenticatedOsuApiV2Interface
{
    private readonly IHttpClientFactory _httpClientFactory;

    public AuthenticatedOsuApiV2Interface(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    private async Task<HttpClient> CreateAuthenticatedClientAsync(IOsuApiV2SessionProvider sessionProvider, CancellationToken cancellationToken = default)
    {
        string token = await sessionProvider.GetOsuApiV2TokenAsync(cancellationToken) ?? throw new NotAuthenticatedException();
        
        HttpClient client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Bearer {token}");
        
        return client;
    }

    public async Task<OsuUser?> SearchUserAsync(IOsuApiV2SessionProvider sessionProvider, string searchString, string mode = "osu", CancellationToken cancellationToken = default)
    { 
        HttpClient client = await CreateAuthenticatedClientAsync(sessionProvider, cancellationToken);
        HttpResponseMessage response =
            await client.GetAsync($"https://osu.ppy.sh/api/v2/users/{searchString}/{mode}", cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<OsuUser>(cancellationToken: cancellationToken);
    }
    
    public async Task<OsuUser> MeAsync(IOsuApiV2SessionProvider sessionProvider, string mode = "osu", CancellationToken cancellationToken = default)
    {
        HttpClient client = await CreateAuthenticatedClientAsync(sessionProvider, cancellationToken);
        int osuUserId = (await sessionProvider.GetOsuApiV2UserIdAsync(cancellationToken))!.Value;

        HttpResponseMessage response =
            await client.GetAsync($"https://osu.ppy.sh/api/v2/users/{osuUserId}/{mode}", cancellationToken);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<OsuUser>(cancellationToken: cancellationToken) ?? throw new InvalidDataException();
    }
}