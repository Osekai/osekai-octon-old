using Microsoft.EntityFrameworkCore;
using Osekai.Octon.Database.Dtos;
using Osekai.Octon.Database.EntityFramework.MySql.Models;
using Osekai.Octon.Database.Repositories;

namespace Osekai.Octon.Database.EntityFramework.MySql.Repositories;

public class MySqlEntityFrameworkAppRepository: IAppRepository
{
    private readonly MySqlOsekaiDbContext _context;
    
    internal MySqlEntityFrameworkAppRepository(MySqlOsekaiDbContext context)
    {
        _context = context;
    }

    public async Task<AppDto?> GetAppByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        IQueryable<App> queryable = _context.Apps.AsNoTracking().Include(app => app.AppTheme).Where(app => app.Id == id);
        App? app = await queryable.FirstOrDefaultAsync(cancellationToken);
        
        return app?.ToDto();
    }

}