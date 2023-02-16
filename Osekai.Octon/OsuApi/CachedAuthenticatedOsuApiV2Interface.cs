using Osekai.Octon.Exceptions;
using Osekai.Octon.OsuApi.Payloads;

namespace Osekai.Octon.OsuApi;

public class CachedAuthenticatedOsuApiV2Interface: IAuthenticatedOsuApiV2Interface
{
    private readonly IAuthenticatedOsuApiV2Interface _authenticatedOsuApiV2Interface;
    private readonly ICache _cache;

    public CachedAuthenticatedOsuApiV2Interface(ICache cache,
        IAuthenticatedOsuApiV2Interface authenticatedOsuApiV2Interface)
    {
        _authenticatedOsuApiV2Interface = authenticatedOsuApiV2Interface;
        _cache = cache;
    } 
    
    public async Task<OsuUser?> SearchUserAsync(IOsuApiV2SessionProvider sessionProvider, string searchString, string mode = "osu", CancellationToken cancellationToken = default)
    {
        string entryName = $"osu_api_user_{searchString}_{mode}";
        OsuUser? user = await _cache.GetAsync<OsuUser>(entryName);
        
        if (user == null)
        {
            user = await _authenticatedOsuApiV2Interface.SearchUserAsync(sessionProvider, searchString, mode, cancellationToken);
            await _cache.SetAsync(entryName, user, cancellationToken: cancellationToken);
        }   

        return user;
    }

    public async Task<OsuUser> MeAsync(IOsuApiV2SessionProvider sessionProvider, string mode = "osu", CancellationToken cancellationToken = default)
    {
        int osuUserId = await sessionProvider.GetOsuApiV2UserIdAsync() ?? throw new NotAuthenticatedException();
        return (await SearchUserAsync(sessionProvider, osuUserId.ToString(), mode, cancellationToken))!;
    }
}