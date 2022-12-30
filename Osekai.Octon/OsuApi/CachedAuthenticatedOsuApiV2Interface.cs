using Osekai.Octon.Exceptions;
using Osekai.Octon.OsuApi.Payloads;

namespace Osekai.Octon.OsuApi;

public class CachedAuthenticatedOsuApiV2Interface: IAuthenticatedOsuApiV2Interface
{
    private readonly IAuthenticatedOsuApiV2Interface _authenticatedOsuApiV2Interface;
    private readonly ICache _cache;

    private readonly CurrentSession _currentSession;
    
    public CachedAuthenticatedOsuApiV2Interface(ICache cache,
        CurrentSession currentSession,
        IAuthenticatedOsuApiV2Interface authenticatedOsuApiV2Interface)
    {
        _authenticatedOsuApiV2Interface = authenticatedOsuApiV2Interface;
        _currentSession = currentSession;
        _cache = cache;
    } 
    
    public async Task<OsuUser?> SearchUserAsync(string searchString, string mode = "osu", CancellationToken cancellationToken = default)
    {
        if (_currentSession.IsNull())
            throw new NotAuthenticatedException();
        
        string entryName = $"osu_api_user_{searchString}_{mode}";
        OsuUser? user = await _cache.GetAsync<OsuUser>(entryName);
        
        if (user == null)
        {
            user = await _authenticatedOsuApiV2Interface.SearchUserAsync(searchString, mode, cancellationToken);
            await _cache.SetAsync(entryName, user);
        }

        return user;
    }

    public Task<OsuUser> MeAsync(string mode = "osu", CancellationToken cancellationToken = default)
    {
        if (_currentSession.IsNull())
            throw new NotAuthenticatedException();

        return SearchUserAsync(_currentSession.OsuUserId!.Value.ToString(), mode, cancellationToken)!;
    }
}