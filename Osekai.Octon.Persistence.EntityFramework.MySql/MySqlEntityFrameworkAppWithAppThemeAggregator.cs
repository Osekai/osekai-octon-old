using Microsoft.EntityFrameworkCore;
using Osekai.Octon.Objects.Aggregators;
using Osekai.Octon.Persistence.EntityFramework.MySql.Dtos;
using Osekai.Octon.Persistence.EntityFramework.MySql.Models;

namespace Osekai.Octon.Persistence.EntityFramework.MySql;

public class MySqlEntityFrameworkAppWithAppThemeAggregator: IAggregator<IReadOnlyAppWithAppTheme>
{
    protected MySqlOsekaiDbContext Context { get; }
    
    public MySqlEntityFrameworkAppWithAppThemeAggregator(MySqlOsekaiDbContext context)
    {
        Context = context;
    }
    
    public async Task<IEnumerable<IReadOnlyAppWithAppTheme>> AggregateAllAsync(CancellationToken cancellationToken)
    {
        IEnumerable<App> app = await Context.Apps.Include(e => e.AppTheme).ToArrayAsync(cancellationToken);
        return app.Select(e => new AppWithAppThemeDto { App = e.ToDto(), AppTheme = e.AppTheme?.ToDto()});
    }
}