using Microsoft.EntityFrameworkCore.Storage;

namespace Osekai.Octon.Database.EntityFramework;

public class EntityFrameworkTransaction: ITransaction
{
    internal EntityFrameworkTransaction(IDbContextTransaction transaction)
    {
        _transaction = transaction;
    }
    
    private readonly IDbContextTransaction _transaction;
    
    public Task RollbackAsync(CancellationToken cancellationToken = default) =>
        _transaction.RollbackAsync(cancellationToken);

    public Task CommitAsync(CancellationToken cancellationToken = default) =>
        _transaction.CommitAsync(cancellationToken);

    public ValueTask DisposeAsync() => _transaction.DisposeAsync();
    
    public void Dispose() => _transaction.Dispose();
}