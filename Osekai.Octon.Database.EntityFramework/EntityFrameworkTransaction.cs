using System.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace Osekai.Octon.Database.EntityFramework;

public delegate void EntityFrameworkTransactionDisposedCallback();

public class EntityFrameworkTransaction: ITransaction
{
    public readonly struct SubordinateTransactionHandle
    {
        private readonly EntityFrameworkTransaction _transaction;
        
        public SubordinateTransactionHandle(EntityFrameworkTransaction transaction)
        {
            _transaction = transaction;
        }

        public void Detach()
        {
            Interlocked.Decrement(ref _transaction._pendingSubordinateTransactionCount);
        }
    }
    
    private readonly EntityFrameworkTransactionDisposedCallback _callback;
    private int _pendingSubordinateTransactionCount;

    internal EntityFrameworkTransaction(EntityFrameworkTransactionDisposedCallback callback, IDbContextTransaction transaction)
    {
        _callback = callback;
        _transaction = transaction;
    }
    
    private readonly IDbContextTransaction _transaction;

    public EntityFrameworkSubordinateTransaction CreateSubordinateTransaction()
    {
        Interlocked.Increment(ref _pendingSubordinateTransactionCount);
        return new EntityFrameworkSubordinateTransaction(new SubordinateTransactionHandle(this));
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        if (_pendingSubordinateTransactionCount != 0)
            throw new InvalidOperationException("There are subordinate transactions pending, the transaction cannot be committed");

        await _transaction.CommitAsync(cancellationToken);
    }

    private volatile bool _disposed;

    public async ValueTask DisposeAsync()
    {
        if (_pendingSubordinateTransactionCount != 0)
            throw new InvalidOperationException("There are subordinate transactions pending, the transaction cannot be disposed");
        
        if (_disposed)
            return;

        _disposed = true;

        await _transaction.DisposeAsync();
        _callback.Invoke();
    }
    
    public void Dispose()
    {      
        if (_pendingSubordinateTransactionCount != 0)
            throw new InvalidOperationException("There are subordinate transactions pending, the transaction cannot be disposed");

        if (_disposed)
            return;

        _disposed = true;
        
        _transaction.Dispose();
        _callback.Invoke();
    }
}