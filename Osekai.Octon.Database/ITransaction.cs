using System.Data;

namespace Osekai.Octon.Database;

public interface ITransaction: IAsyncDisposable, IDisposable
{
    Task CommitAsync(CancellationToken cancellationToken = default);
}