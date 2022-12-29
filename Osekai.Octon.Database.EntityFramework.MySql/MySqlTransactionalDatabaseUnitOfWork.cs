using Osekai.Octon.Database.Repositories;

namespace Osekai.Octon.Database.EntityFramework.MySql;

public class MySqlTransactionalDatabaseUnitOfWork: MySqlDatabaseUnitOfWork, IDatabaseTransactionalUnitOfWork
{
    private readonly ITransaction _transaction;
    
    public MySqlTransactionalDatabaseUnitOfWork(ITransaction transaction, MySqlOsekaiDbContext context) : base(context)
    {
        _transaction = transaction;
    }

    public ValueTask DisposeAsync() => _transaction.DisposeAsync();
    public void Dispose() => _transaction.Dispose();
    
    public Task CommitAsync(CancellationToken cancellationToken) => _transaction.CommitAsync(cancellationToken);
}