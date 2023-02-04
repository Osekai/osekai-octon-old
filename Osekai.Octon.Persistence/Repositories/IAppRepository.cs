using Osekai.Octon.Objects;

namespace Osekai.Octon.Persistence.Repositories;

public interface IAppRepository
{
    Task<IReadOnlyApp?> GetAppByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> PatchAppAsync(int id, int order, string name, string simpleName, bool visible,
        bool experimental, CancellationToken cancellationToken = default);

    Task<IEnumerable<IReadOnlyApp>> GetAppsAsync(CancellationToken cancellationToken = default);
}