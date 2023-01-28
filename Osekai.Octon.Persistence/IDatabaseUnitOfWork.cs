using System.Data;
using Osekai.Octon.Persistence.Repositories;

namespace Osekai.Octon.Persistence;

public interface IDatabaseUnitOfWork: IAsyncDisposable, IDisposable
{
    IAppRepository AppRepository { get; }
    ISessionRepository SessionRepository { get; }
    ICacheEntryRepository CacheEntryRepository { get; }
    IMedalRepository MedalRepository { get; }
    IUserGroupRepository UserGroupRepository { get; }
    IAppThemeRepository AppThemeRepository { get; }
    IUserPermissionsOverrideRepository UserPermissionsOverrideRepository { get; }
    IMedalSettingsRepository MedalSettingsRepository { get; }
    IMedalSolutionRepository MedalSolutionRepository { get; }
    IBeatmapPackRepository BeatmapPackRepository { get; }
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<ITransaction> BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.Serializable, CancellationToken cancellationToken = default);
}