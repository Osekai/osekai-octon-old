
using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Osekai.Octon.Domain.Repositories;

namespace Osekai.Octon.Persistence.EntityFramework;

public abstract class EntityFrameworkUnitOfWork<T>: IUnitOfWork where T: DbContext
{
    protected T Context { get; }

    protected EntityFrameworkUnitOfWork(T context)
    {
        Context = context;
    }

    public abstract IAppRepository AppRepository { get; }
    public abstract ISessionRepository SessionRepository { get; }
    public abstract IMedalRepository MedalRepository { get; }
    public abstract IUserGroupRepository UserGroupRepository { get; }
    public abstract IUserPermissionsOverrideRepository UserPermissionsOverrideRepository { get; }
    public abstract IBeatmapPackRepository BeatmapPackRepository { get; }
    public abstract ILocaleRepository LocaleRepository { get; }
    public abstract ITeamMemberRepository TeamMemberRepository { get; }

    public virtual async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await Context.SaveChangesAsync(cancellationToken);
    }
}