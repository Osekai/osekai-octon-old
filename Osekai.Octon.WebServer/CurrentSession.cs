using Osekai.Octon.Database.Dtos;
using Osekai.Octon.Database.HelperTypes;
using Osekai.Octon.Exceptions;
using Osekai.Octon.OsuApi;
using Osekai.Octon.Services;

namespace Osekai.Octon.WebServer;

public class CurrentSession: IOsuApiV2SessionProvider
{
    private IOsuApiV2SessionProvider? _session;

    public void Set(IOsuApiV2SessionProvider session)
    {
        _session = session;
    }

    public bool IsNull() => _session == null;

    public Task<string?> GetOsuApiV2TokenAsync(CancellationToken cancellationToken = default)
    {
        return _session?.GetOsuApiV2TokenAsync(cancellationToken) ?? Task.FromResult<string?>(null);
    }

    public Task<int?> GetOsuApiV2UserIdAsync(CancellationToken cancellationToken = default)
    {
        return _session?.GetOsuApiV2UserIdAsync(cancellationToken) ?? Task.FromResult<int?>(null);
    }
}