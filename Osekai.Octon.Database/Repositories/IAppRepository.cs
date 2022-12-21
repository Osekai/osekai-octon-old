using Osekai.Octon.Database.EntityFramework;

namespace Osekai.Octon.Database.Repositories;

public interface IAppRepository
{
    Task<App?> GetAppByIdAsync(int id);
}