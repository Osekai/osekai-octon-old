namespace Osekai.Octon.Database;

public interface ITransaction
{
    Task RollbackAsync(CancellationToken cancellationToken = default);
    Task CommitAsync(CancellationToken cancellationToken = default);
}