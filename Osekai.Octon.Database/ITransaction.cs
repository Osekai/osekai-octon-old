namespace Osekai.Octon.Database;

public interface ITransaction: IAsyncDisposable, IDisposable
{
    Task RollbackAsync(CancellationToken cancellationToken = default);
    Task CommitAsync(CancellationToken cancellationToken = default);
}