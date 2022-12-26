using Osekai.Octon.Applications.OsuApiV2.Payloads;

namespace Osekai.Octon.Applications.OsuApiV2;

public interface IAuthenticatedOsuApiV2
{
    Task<User?> SearchUserAsync(string searchString, string mode = "osu",
        CancellationToken cancellationToken = default);

    Task<User?> MeAsync(string mode = "osu", CancellationToken cancellationToken = default);
}