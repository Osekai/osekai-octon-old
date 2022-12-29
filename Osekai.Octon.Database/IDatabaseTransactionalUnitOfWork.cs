namespace Osekai.Octon.Database;

public interface IDatabaseTransactionalUnitOfWork: IDatabaseUnitOfWork, IAsyncDisposable, IDisposable
{
    Task CommitAsync(CancellationToken cancellationToken = default);
}