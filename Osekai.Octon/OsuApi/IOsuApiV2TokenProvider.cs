namespace Osekai.Octon.OsuApi;

public interface IOsuApiV2TokenProvider
{
    Task<string?> GetOsuApiV2TokenAsync(CancellationToken cancellationToken = default);
}