using Osekai.Octon.OsuApi.Payloads;

namespace Osekai.Octon.OsuApi;

public interface IAuthenticatedOsuApiV2Interface
{
    Task<OsuUser?> SearchUserAsync(string searchString, string mode = "osu",
        CancellationToken cancellationToken = default);

    Task<OsuUser> MeAsync(string mode = "osu", CancellationToken cancellationToken = default);
}