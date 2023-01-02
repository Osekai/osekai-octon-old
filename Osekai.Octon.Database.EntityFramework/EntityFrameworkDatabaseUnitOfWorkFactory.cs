using System.Data;

namespace Osekai.Octon.Database.EntityFramework;

public abstract class EntityFrameworkDatabaseUnitOfWorkFactory: IDatabaseUnitOfWorkFactory
{
    protected EntityFrameworkTransactionProvider TransactionProvider { get; }
    
    protected EntityFrameworkDatabaseUnitOfWorkFactory(EntityFrameworkTransactionProvider transactionProvider)
    {
        TransactionProvider = transactionProvider;
    }

    public abstract Task<IDatabaseUnitOfWork> CreateAsync(CancellationToken cancellationToken = default);
    public abstract Task<IDatabaseTransactionalUnitOfWork> CreateTransactionalAsync(CancellationToken cancellationToken = default);
}