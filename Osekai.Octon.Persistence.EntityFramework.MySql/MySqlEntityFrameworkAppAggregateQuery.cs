using Microsoft.EntityFrameworkCore;
using Osekai.Octon.Persistence.EntityFramework.MySql.Dtos;
using Osekai.Octon.Persistence.EntityFramework.MySql.Entities;
using Osekai.Octon.Persistence.QueryResults;

namespace Osekai.Octon.Persistence.EntityFramework.MySql;

public class MySqlEntityFrameworkAppAggregateQuery: IQuery<IReadOnlyAppAggregateQueryResult>
{
    protected MySqlOsekaiDbContext Context { get; }
    
    public MySqlEntityFrameworkAppAggregateQuery(MySqlOsekaiDbContext context)
    {
        Context = context;
    }
    
    public async Task<IEnumerable<IReadOnlyAppAggregateQueryResult>> ExecuteAsync(CancellationToken cancellationToken)
    {
        IEnumerable<App> app = await Context.Apps.Include(e => e.AppTheme).ToArrayAsync(cancellationToken);
        return app.Select(e => new AppAggregateQueryResultDto { App = e.ToDto(), AppTheme = e.AppTheme?.ToDto()});
    }
}