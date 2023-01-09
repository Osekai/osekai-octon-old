
using System.Data;
using Osekai.Octon.Database.Repositories;

namespace Osekai.Octon.Database;

public interface IDatabaseUnitOfWork: IAsyncDisposable, IDisposable
{
    IAppRepository AppRepository { get; }
    ISessionRepository SessionRepository { get; }
    ICacheEntryRepository CacheEntryRepository { get; }
    IMedalRepository MedalRepository { get; }
    IUserGroupRepository UserGroupRepository { get; }
    IUserPermissionsOverrideRepository UserPermissionsOverrideRepository { get; }
    Task SaveAsync(CancellationToken cancellationToken = default);
    Task<ITransaction> BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.Serializable, CancellationToken cancellationToken = default);
}