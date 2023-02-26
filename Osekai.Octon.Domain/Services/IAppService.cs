using Osekai.Octon.Domain.AggregateRoots;

namespace Osekai.Octon.Domain.Services;

public interface IAppService
{
    Task<App?> GetAppByIdAsync(int id, bool includeFaqs = false, CancellationToken cancellationToken = default);
    Task<IEnumerable<App>> GetAppsAsync(bool includeFaqs = false, CancellationToken cancellationToken = default);
}