using Osekai.Octon.OsuApi;
using Osekai.Octon.Permissions;
using Osekai.Octon.Domain.Services.Default;
namespace Osekai.Octon.WebServer;

public class CurrentSession: IOsuApiV2SessionProvider
{
    private readonly struct Inner
    {
        public Inner(IOsuApiV2SessionProvider osuApiV2SessionProvider, IPermissionStore permissionStore)
        {
            OsuApiV2SessionProvider = osuApiV2SessionProvider;
            PermissionStore = permissionStore;
        }
        
        public IOsuApiV2SessionProvider OsuApiV2SessionProvider { get; }
        public IPermissionStore PermissionStore { get; }
    }

    private Inner? _inner;

    public void Set(IOsuApiV2SessionProvider osuApiV2SessionProvider, IPermissionStore permissionStore)
    {
        _inner = new Inner(osuApiV2SessionProvider, permissionStore);
    }

    public bool IsNull() => !_inner.HasValue;

    public IPermissionStore? PermissionStore => _inner?.PermissionStore; 

    public Task<string?> GetOsuApiV2TokenAsync(CancellationToken cancellationToken = default)
    {
        return _inner?.OsuApiV2SessionProvider.GetOsuApiV2TokenAsync(cancellationToken) ?? Task.FromResult<string?>(null);
    }

    public Task<int?> GetOsuApiV2UserIdAsync(CancellationToken cancellationToken = default)
    {
        return _inner?.OsuApiV2SessionProvider.GetOsuApiV2UserIdAsync(cancellationToken) ?? Task.FromResult<int?>(null);
    }
}