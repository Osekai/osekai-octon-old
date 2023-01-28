
using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Osekai.Octon.Persistence.Repositories;

namespace Osekai.Octon.Persistence.EntityFramework;

public abstract class EntityFrameworkDatabaseUnitOfWork<T>: IDatabaseUnitOfWork where T: DbContext
{
    protected T Context { get; }

    protected EntityFrameworkDatabaseUnitOfWork(T context)
    {
        Context = context;
    }

    public abstract IAppRepository AppRepository { get; }
    public abstract ISessionRepository SessionRepository { get; }
    public abstract ICacheEntryRepository CacheEntryRepository { get; }
    public abstract IMedalRepository MedalRepository { get; }
    public abstract IUserGroupRepository UserGroupRepository { get; }
    public abstract IAppThemeRepository AppThemeRepository { get; }
    public abstract IUserPermissionsOverrideRepository UserPermissionsOverrideRepository { get; }
    public abstract IMedalSettingsRepository MedalSettingsRepository { get; }
    public abstract IMedalSolutionRepository MedalSolutionRepository { get; }
    public abstract IBeatmapPackRepository BeatmapPackRepository { get; }

    public virtual async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await Context.SaveChangesAsync(cancellationToken);
    }

    public async Task<ITransaction> BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.Serializable, CancellationToken cancellationToken = default)
    {
        IDbContextTransaction transaction = await Context.Database.BeginTransactionAsync(isolationLevel, cancellationToken);
        return new EntityFrameworkTransaction(transaction);
    }

    public void DiscardChanges()
    {
        foreach (var entry in Context.ChangeTracker.Entries())
        {
            switch (entry.State)
            {
                case EntityState.Modified:
                case EntityState.Deleted:
                    entry.State = EntityState.Modified; 
                    entry.State = EntityState.Unchanged;
                    break;
                case EntityState.Added:
                    entry.State = EntityState.Detached;
                    break;
                case EntityState.Detached:
                    break;
                case EntityState.Unchanged:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private volatile bool _disposed;
    
    public ValueTask DisposeAsync()
    {
        if (_disposed)
            return ValueTask.CompletedTask;

        _disposed = true;
        
        return Context.DisposeAsync();
    }

    public void Dispose()
    {
        if (_disposed)
            return;

        _disposed = true;
        
        Context.Dispose();
    }
}