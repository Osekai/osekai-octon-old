using System.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace Osekai.Octon.Database.EntityFramework;

public class EntityFrameworkTransaction: ITransaction
{
    internal EntityFrameworkTransaction(IDbContextTransaction transaction)
    {
        _transaction = transaction;
    }
    
    private readonly IDbContextTransaction _transaction;

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await _transaction.CommitAsync(cancellationToken);
    }

    private volatile bool _disposed;

    public async ValueTask DisposeAsync()
    {
        if (_disposed)
            return;

        _disposed = true;

        await _transaction.DisposeAsync();
    }
    
    public void Dispose()
    {   
        if (_disposed)
            return;

        _disposed = true;
        
        _transaction.Dispose();
    }
}