using System.Data;
using Microsoft.EntityFrameworkCore;

namespace Osekai.Octon.Database.EntityFramework.MySql;

public class MySqlDatabaseUnitOfWorkFactory: IDatabaseUnitOfWorkFactory
{
    private IDbContextFactory<MySqlOsekaiDbContext> _dbContextFactory;
    
    public MySqlDatabaseUnitOfWorkFactory(IDbContextFactory<MySqlOsekaiDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<IDatabaseUnitOfWork> CreateAsync(CancellationToken cancellationToken = default)
    {
        return new MySqlDatabaseUnitOfWork(await _dbContextFactory.CreateDbContextAsync(cancellationToken));
    }
}