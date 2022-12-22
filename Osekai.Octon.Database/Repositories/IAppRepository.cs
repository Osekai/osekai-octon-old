using Osekai.Octon.Database.EntityFramework;
using Osekai.Octon.Database.Models;

namespace Osekai.Octon.Database.Repositories;

public interface IAppRepository
{
    Task<App?> GetAppByIdAsync(int id, bool includeTheme = false, CancellationToken cancellationToken = default);
}