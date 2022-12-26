namespace Osekai.Octon.Applications;

public interface IOsuApiV2TokenProvider
{
    Task<string?> GetOsuApiV2TokenAsync(CancellationToken cancellationToken = default);
}