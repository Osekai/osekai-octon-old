using Microsoft.EntityFrameworkCore;

namespace Osekai.Octon.Persistence.EntityFramework.MySql;

public class MySqlTestDataPopulator: ITestDataPopulator
{
    protected MySqlOsekaiDbContext Context { get; }
    
    public MySqlTestDataPopulator(MySqlOsekaiDbContext context)
    {
        Context = context;
    }
    
    public async Task PopulateDatabaseAsync(CancellationToken cancellationToken = default)
    {
        await Context.Database.EnsureDeletedAsync(cancellationToken);
        await Context.Database.MigrateAsync(cancellationToken);

        await Context.Database.ExecuteSqlRawAsync(MySqlDataPopulatorResources.Sql);
        await Context.SaveChangesAsync(cancellationToken);
    }
}