using Osekai.Octon.Persistence.Dtos;

namespace Osekai.Octon.Persistence.Repositories;

public interface IAppThemeRepository
{
    Task<AppThemeDto?> GetAppThemeByAppIdAsync(int appId, CancellationToken cancellationToken = default);
}