using Osekai.Octon.Models;

namespace Osekai.Octon.Persistence.Repositories;

public interface IAppThemeRepository
{
    Task<IReadOnlyAppTheme?> GetAppThemeByAppIdAsync(int appId, CancellationToken cancellationToken = default);
}