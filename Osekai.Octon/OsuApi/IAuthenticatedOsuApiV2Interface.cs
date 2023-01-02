using Osekai.Octon.OsuApi.Payloads;

namespace Osekai.Octon.OsuApi;

public interface IAuthenticatedOsuApiV2Interface
{
    Task<OsuUser?> SearchUserAsync(IOsuApiV2SessionProvider sessionProvider, string searchString, string mode = "osu",
        CancellationToken cancellationToken = default);

    Task<OsuUser> MeAsync(IOsuApiV2SessionProvider sessionProvider, string mode = "osu", CancellationToken cancellationToken = default);
}