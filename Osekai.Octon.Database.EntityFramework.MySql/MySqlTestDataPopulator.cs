using Microsoft.EntityFrameworkCore;
using Osekai.Octon.Database.Enums;

namespace Osekai.Octon.Database.EntityFramework.MySql;

public class MySqlTestDataPopulator: ITestDataPopulator
{
    private readonly IDbContextFactory<MySqlOsekaiDbContext> _contextFactory;
    
    public MySqlTestDataPopulator(IDbContextFactory<MySqlOsekaiDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }
    
    public async Task PopulateDatabaseAsync(CancellationToken cancellationToken = default)
    {
        MySqlOsekaiDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        
        await context.Database.EnsureDeletedAsync(cancellationToken);
        await context.Database.MigrateAsync(cancellationToken);

        await context.Database.ExecuteSqlRawAsync(MySqlDataPopulatorResources.Sql);
        await context.SaveChangesAsync(cancellationToken);
    }
}