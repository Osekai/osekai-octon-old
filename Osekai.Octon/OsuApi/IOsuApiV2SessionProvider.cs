namespace Osekai.Octon.OsuApi;

public interface IOsuApiV2SessionProvider
{
    Task<string?> GetOsuApiV2TokenAsync(CancellationToken cancellationToken = default);
    Task<int?> GetOsuApiV2UserIdAsync(CancellationToken cancellationToken = default);
}