using Microsoft.EntityFrameworkCore;
using Osekai.Octon.Database.Repositories;

namespace Osekai.Octon.Database.EntityFramework.Repositories;

public class MySqlEntityFrameworkAppRepository: IAppRepository
{
    private readonly MySqlOsekaiDbContext _context;
    
    public MySqlEntityFrameworkAppRepository(MySqlOsekaiDbContext context)
    {
        _context = context;
    }
    
    public Task<App?> GetAppByIdAsync(int id) =>
        _context.Apps.Where(app => app.Id == id).FirstOrDefaultAsync();
}