using Microsoft.EntityFrameworkCore;
using Osekai.Octon.Database.Models;
using Osekai.Octon.Database.Repositories;
using Osekai.Octon.Database.Repositories.Query;

namespace Osekai.Octon.Database.EntityFramework.Repositories;

public class MySqlEntityFrameworkAppRepository: IAppRepository
{
    private readonly MySqlOsekaiDbContext _context;
    
    internal MySqlEntityFrameworkAppRepository(MySqlOsekaiDbContext context)
    {
        _context = context;
    }

    public Task<App?> GetAppByIdAsync(GetAppByIdQuery query, CancellationToken cancellationToken = default)
    {
        IQueryable<App> queryable = _context.Apps.AsNoTracking().Where(app => app.Id == query.Id);

        if (query.IncludeTheme)
            queryable = queryable.Include(app => app.AppTheme);

        return queryable.FirstOrDefaultAsync(cancellationToken);
    }

}