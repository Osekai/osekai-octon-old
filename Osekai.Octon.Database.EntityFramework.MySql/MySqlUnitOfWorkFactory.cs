using System.Data;
using Microsoft.EntityFrameworkCore;

namespace Osekai.Octon.Database.EntityFramework;

public class MySqlUnitOfWorkFactory: IUnitOfWorkFactory
{
    private readonly MySqlOsekaiDbContext _context;
    private readonly ITransactionProvider _transactionProvider;
    
    public MySqlUnitOfWorkFactory(MySqlOsekaiDbContext context, ITransactionProvider transactionProvider)
    {
        _context = context;
        _transactionProvider = transactionProvider;
    }
    
    public Task<IUnitOfWork> CreateAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult((IUnitOfWork)new MySqlUnitOfWork(_context));
    }

    public async Task<ITransactionalUnitOfWork> CreateTransactional(
        IsolationLevel isolationLevel = IsolationLevel.Serializable,
        CancellationToken cancellationToken = default)
    {
        ITransaction transaction = await _transactionProvider.BeginTransactionAsync(isolationLevel, cancellationToken);
        return new EntityFrameworkTransactionalUnitOfWork(new MySqlUnitOfWork(_context), transaction);
    }
}