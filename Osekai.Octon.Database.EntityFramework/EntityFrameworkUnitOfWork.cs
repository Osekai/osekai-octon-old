using System.Data;
using Microsoft.EntityFrameworkCore;
using Osekai.Octon.Database.Repositories;

namespace Osekai.Octon.Database.EntityFramework;

public abstract class EntityFrameworkUnitOfWork<T>: IUnitOfWork where T: DbContext
{
    protected T Context { get; }
    
    private readonly EntityFrameworkTransactionProvider _transactionProvider;
    
    private ITransaction? _transaction;
    
    protected EntityFrameworkUnitOfWork(T context)
    {
        Context = context;
        _transactionProvider = new EntityFrameworkTransactionProvider(context);
    }

    public abstract IAppRepository AppRepository { get; }

    public async Task BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.Serializable,
        CancellationToken cancellationToken = default)
    {
        _transaction = await _transactionProvider.BeginTransactionAsync(isolationLevel, cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
            throw new InvalidOperationException("The transaction was not begun");

        await _transaction.CommitAsync(cancellationToken);
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
            throw new InvalidOperationException("The transaction was not begun");

        await _transaction.RollbackAsync(cancellationToken);
    }

    public virtual async Task SaveAsync(CancellationToken cancellationToken)
    {
        await Context.SaveChangesAsync(cancellationToken);
    }

    public ValueTask DisposeAsync() =>_transaction?.DisposeAsync() ?? ValueTask.CompletedTask;

    public void Dispose() => _transaction?.Dispose();
}