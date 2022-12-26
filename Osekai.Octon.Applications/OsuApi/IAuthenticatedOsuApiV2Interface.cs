using Osekai.Octon.Applications.OsuApi.Payloads;

namespace Osekai.Octon.Applications.OsuApi;

public interface IAuthenticatedOsuApiV2Interface
{
    Task<User?> SearchUserAsync(string searchString, string mode = "osu",
        CancellationToken cancellationToken = default);

    Task<User?> MeAsync(string mode = "osu", CancellationToken cancellationToken = default);
}