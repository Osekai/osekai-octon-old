using System.Data;
using Osekai.Octon.Database.Repositories;

namespace Osekai.Octon.Database;

public interface IDatabaseUnitOfWork
{
    public IAppRepository AppRepository { get; }
    public ISessionRepository SessionRepository { get; }
    public ICacheEntryRepository CacheEntryRepository { get; }

    Task SaveAsync(CancellationToken cancellationToken = default);
    void DiscardChanges();
}