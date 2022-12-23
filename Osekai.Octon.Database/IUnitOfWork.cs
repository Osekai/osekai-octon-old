using System.Data;
using Osekai.Octon.Database.Repositories;

namespace Osekai.Octon.Database;

public interface IUnitOfWork: IAsyncDisposable, IDisposable
{
    public IAppRepository AppRepository { get; }

    Task BeginTransactionAsync(
        IsolationLevel isolationLevel = IsolationLevel.Serializable, 
        CancellationToken cancellationToken = default);

    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    
    Task SaveAsync(CancellationToken cancellationToken = default);
}