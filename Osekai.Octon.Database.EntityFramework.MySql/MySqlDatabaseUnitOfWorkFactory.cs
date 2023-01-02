using System.Data;

namespace Osekai.Octon.Database.EntityFramework.MySql;

public class MySqlDatabaseUnitOfWorkFactory: EntityFrameworkDatabaseUnitOfWorkFactory
{
    private readonly MySqlOsekaiDbContext _context;
    
    public MySqlDatabaseUnitOfWorkFactory(MySqlOsekaiDbContext context, EntityFrameworkTransactionProvider transactionProvider) : base(transactionProvider)
    {
        _context = context;
    }

    public override Task<IDatabaseUnitOfWork> CreateAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult((IDatabaseUnitOfWork)new MySqlDatabaseUnitOfWork(_context));
    }

    public override async Task<IDatabaseTransactionalUnitOfWork> CreateTransactionalAsync(CancellationToken cancellationToken = default)
    {
        ITransaction transaction = await TransactionProvider.BeginTransactionAsync(cancellationToken);
        return new MySqlTransactionalDatabaseUnitOfWork(transaction, _context);
    }
}