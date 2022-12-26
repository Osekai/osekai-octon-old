using Osekai.Octon.Applications.OsuApi.Payloads;
using Osekai.Octon.Services;

namespace Osekai.Octon.Applications.OsuApi;

public class CachedAuthenticatedOsuApiV2Interface: IAuthenticatedOsuApiV2Interface
{
    private readonly IAuthenticatedOsuApiV2Interface _authenticatedOsuApiV2Interface;
    private readonly ICache _cache;
    
    public CachedAuthenticatedOsuApiV2Interface(ICache cache, IAuthenticatedOsuApiV2Interface authenticatedOsuApiV2Interface)
    {
        _authenticatedOsuApiV2Interface = authenticatedOsuApiV2Interface;
        _cache = cache;
    } 
    
    public async Task<User?> SearchUserAsync(string searchString, string mode = "osu", CancellationToken cancellationToken = default)
    {
        string entryName = $"osu_api_user_{searchString}_{mode}";
        User? user = await _cache.GetAsync<User>(entryName);
        
        if (user == null)
        {
            user = await _authenticatedOsuApiV2Interface.SearchUserAsync(searchString, mode, cancellationToken);
            await _cache.SetAsync(entryName, user);
        }

        return user;
    }

    public Task<User?> MeAsync(string mode = "osu", CancellationToken cancellationToken = default)
    {
        return _authenticatedOsuApiV2Interface.MeAsync(mode, cancellationToken);
    }
}