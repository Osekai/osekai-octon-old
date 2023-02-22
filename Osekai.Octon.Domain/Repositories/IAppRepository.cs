using Osekai.Octon.Domain.Aggregates;

namespace Osekai.Octon.Domain.Repositories;

public interface IAppRepository
{
    Task<App?> GetAppByIdAsync(int id, bool includeFaqs = false, CancellationToken cancellationToken = default);
    Task<bool> SaveAppAsync(App app, CancellationToken cancellationToken = default);
    Task<IEnumerable<App>> GetAppsAsync(bool includeFaqs = false, CancellationToken cancellationToken = default);
}