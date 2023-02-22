using System.Data;
using Osekai.Octon.Domain.Repositories;

namespace Osekai.Octon.Persistence;

public interface IUnitOfWork
{
    IAppRepository AppRepository { get; }
    ISessionRepository SessionRepository { get; }
    IMedalRepository MedalRepository { get; }
    IUserGroupRepository UserGroupRepository { get; }
    IUserPermissionsOverrideRepository UserPermissionsOverrideRepository { get; }
    IBeatmapPackRepository BeatmapPackRepository { get; }
    ILocaleRepository LocaleRepository { get; }
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}