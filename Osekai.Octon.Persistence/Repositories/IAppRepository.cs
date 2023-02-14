using Osekai.Octon.Models;

namespace Osekai.Octon.Persistence.Repositories;

public interface IAppRepository
{
    Task<IReadOnlyApp?> GetAppByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> SaveAppAsync(IReadOnlyApp app, CancellationToken cancellationToken = default);
    Task<IEnumerable<IReadOnlyApp>> GetAppsAsync(CancellationToken cancellationToken = default);
}