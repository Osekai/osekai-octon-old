using Osekai.Octon.Persistence.Dtos;

namespace Osekai.Octon.Persistence.Repositories;

public interface IAppRepository
{
    Task<AppDto?> GetAppByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> PatchAppAsync(int id, int order, string name, string simpleName, bool visible,
        bool experimental, CancellationToken cancellationToken = default);
}