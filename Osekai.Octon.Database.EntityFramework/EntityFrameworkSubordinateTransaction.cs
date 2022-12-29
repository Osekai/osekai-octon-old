using System.Data;

namespace Osekai.Octon.Database.EntityFramework;

public class EntityFrameworkSubordinateTransaction: ITransaction
{
    private readonly EntityFrameworkTransaction.SubordinateTransactionHandle _transactionHandle;
    
    public EntityFrameworkSubordinateTransaction(
        EntityFrameworkTransaction.SubordinateTransactionHandle transactionHandle)
    {
        _transactionHandle = transactionHandle;
    }

    private volatile bool _disposed;

    public ValueTask DisposeAsync()
    {
        Dispose();
        return ValueTask.CompletedTask;
    }

    private volatile bool _detached;

    private bool TryDetach()
    {
        if (_detached)
            return false;

        _detached = true;
        
        _transactionHandle.Detach();
        return true;
    }
    
    public void Dispose()
    {
        if (_disposed)
            return;

        _disposed = true;

        TryDetach();
    }

    public Task CommitAsync(CancellationToken cancellationToken = default)
    {
        if (!TryDetach())
            throw new InvalidOperationException("The transaction is already detached");
        
        return Task.CompletedTask;
    }
}