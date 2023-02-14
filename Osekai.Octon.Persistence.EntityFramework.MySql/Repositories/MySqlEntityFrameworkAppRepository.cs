using Microsoft.EntityFrameworkCore;
using Osekai.Octon.Models;
using Osekai.Octon.Persistence.EntityFramework.MySql.Dtos;
using Osekai.Octon.Persistence.EntityFramework.MySql.Entities;
using Osekai.Octon.Persistence.Repositories;

namespace Osekai.Octon.Persistence.EntityFramework.MySql.Repositories;

public class MySqlEntityFrameworkAppRepository: IAppRepository
{
    protected MySqlOsekaiDbContext Context { get; }
    internal MySqlEntityFrameworkAppRepository(MySqlOsekaiDbContext context)
    {
        Context = context;
    }

    public async Task<IReadOnlyApp?> GetAppByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        App? app = await Context.Apps.FindAsync(new object[] {id}, cancellationToken);
        
        return app?.ToDto();
    }

    public async Task<bool> SaveAppAsync(IReadOnlyApp app, CancellationToken cancellationToken = default)
    {
        App? appEntity = await Context.Apps.FindAsync(new object?[] { app.Id }, cancellationToken);
        if (appEntity == null)
            return false;
        
        appEntity.Name = app.Name;
        appEntity.SimpleName = app.SimpleName;
        appEntity.Order = app.Order;
        appEntity.Visible = app.Visible;
        appEntity.Experimental = app.Experimental;
        
        return true;
    }

    public async Task<IEnumerable<IReadOnlyApp>> GetAppsAsync(CancellationToken cancellationToken = default)
    {
        IEnumerable<App> app = await Context.Apps.ToArrayAsync(cancellationToken);
        return app.Select(app => app.ToDto());
    }
}