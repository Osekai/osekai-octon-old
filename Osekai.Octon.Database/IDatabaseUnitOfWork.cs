using System.Transactions;
using Osekai.Octon.Database.Repositories;

namespace Osekai.Octon.Database;

public interface IDatabaseUnitOfWork
{
    IAppRepository AppRepository { get; }
    ISessionRepository SessionRepository { get; }
    ICacheEntryRepository CacheEntryRepository { get; }
    IMedalRepository MedalRepository { get; }
    Task SaveAsync(CancellationToken cancellationToken = default);
    void DiscardChanges();
}