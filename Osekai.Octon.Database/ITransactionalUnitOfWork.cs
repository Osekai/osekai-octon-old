namespace Osekai.Octon.Database;

public interface ITransactionalUnitOfWork: IUnitOfWork, IAsyncDisposable, IDisposable
{
    Task CommitAsync(CancellationToken cancellationToken = default);
    Task RollbackAsync(CancellationToken cancellationToken = default);
}