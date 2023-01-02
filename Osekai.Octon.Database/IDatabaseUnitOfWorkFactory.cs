using System.Data;

namespace Osekai.Octon.Database;

public interface IDatabaseUnitOfWorkFactory
{
    Task<IDatabaseUnitOfWork> CreateAsync(CancellationToken cancellationToken = default);
    Task<IDatabaseTransactionalUnitOfWork> CreateTransactionalAsync(CancellationToken cancellationToken = default);
}