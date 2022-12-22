using System.Data;

namespace Osekai.Octon.Database;

public interface IUnitOfWorkFactory
{
    Task<IUnitOfWork> CreateAsync(CancellationToken cancellationToken = default);
    Task<ITransactionalUnitOfWork> CreateTransactional(
        IsolationLevel isolationLevel = IsolationLevel.Serializable,
        CancellationToken cancellationToken = default);
}