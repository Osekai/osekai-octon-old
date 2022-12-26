﻿using System.Data;
using Microsoft.EntityFrameworkCore;
using Osekai.Octon.Database.Repositories;

namespace Osekai.Octon.Database.EntityFramework;

public abstract class EntityFrameworkUnitOfWork<T>: IUnitOfWork where T: DbContext
{
    protected T Context { get; }

    protected EntityFrameworkUnitOfWork(T context)
    {
        Context = context;
    }

    public abstract IAppRepository AppRepository { get; }
    public abstract ISessionRepository SessionRepository { get; }

    public virtual async Task SaveAsync(CancellationToken cancellationToken)
    {
        await Context.SaveChangesAsync(cancellationToken);
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
}