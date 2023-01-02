using Osekai.Octon.Database.Dtos;

namespace Osekai.Octon.Database.Repositories;

public interface IAppRepository
{
    Task<AppDto?> GetAppByIdAsync(int id, CancellationToken cancellationToken = default);
}