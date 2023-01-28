namespace Osekai.Octon.Persistence;

public interface ITransaction: IAsyncDisposable, IDisposable
{
    Task CommitAsync(CancellationToken cancellationToken = default);
}