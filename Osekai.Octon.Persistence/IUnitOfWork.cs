using System.Data;
using Osekai.Octon.Persistence.Repositories;

namespace Osekai.Octon.Persistence;

public interface IUnitOfWork
{
    IAppRepository AppRepository { get; }
    ISessionRepository SessionRepository { get; }
    IMedalRepository MedalRepository { get; }
    IUserGroupRepository UserGroupRepository { get; }
    IAppThemeRepository AppThemeRepository { get; }
    IUserPermissionsOverrideRepository UserPermissionsOverrideRepository { get; }
    IMedalSettingsRepository MedalSettingsRepository { get; }
    IMedalSolutionRepository MedalSolutionRepository { get; }
    IBeatmapPackRepository BeatmapPackRepository { get; }
    ILocaleRepository LocaleRepository { get; }
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}