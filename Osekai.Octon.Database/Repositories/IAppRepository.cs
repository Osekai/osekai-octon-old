using Osekai.Octon.Database.Models;
using Osekai.Octon.Database.Repositories.Query;

namespace Osekai.Octon.Database.Repositories;

public interface IAppRepository
{
    Task<App?> GetAppByIdAsync(GetAppByIdQuery query, CancellationToken cancellationToken = default);
}