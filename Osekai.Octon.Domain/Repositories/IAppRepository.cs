using Osekai.Octon.Domain.Aggregates;

namespace Osekai.Octon.Domain.Repositories;

public interface IAppRepository
{
    Task<App?> GetAppByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> SaveAppAsync(App app, CancellationToken cancellationToken = default);
    Task<IEnumerable<App>> GetAppsAsync(CancellationToken cancellationToken = default);
}