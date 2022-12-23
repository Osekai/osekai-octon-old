using System.Data;
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
    
    public virtual async Task SaveAsync(CancellationToken cancellationToken)
    {
        await Context.SaveChangesAsync(cancellationToken);
    }
}