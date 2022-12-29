using System.Data;

namespace Osekai.Octon.Database;

public interface IDatabaseUnitOfWorkFactory
{
    Task<IDatabaseUnitOfWork> Create(CancellationToken cancellationToken = default);
    Task<IDatabaseTransactionalUnitOfWork> CreateTransactional(CancellationToken cancellationToken = default);
}