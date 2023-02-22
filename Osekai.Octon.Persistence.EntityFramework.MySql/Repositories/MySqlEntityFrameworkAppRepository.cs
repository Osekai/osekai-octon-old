using Microsoft.EntityFrameworkCore;
using Osekai.Octon.Domain.Entities;
using Osekai.Octon.Domain.Repositories;
using App = Osekai.Octon.Domain.Aggregates.App;

namespace Osekai.Octon.Persistence.EntityFramework.MySql.Repositories;

public class MySqlEntityFrameworkAppRepository: IAppRepository
{
    protected MySqlOsekaiDbContext Context { get; }
    internal MySqlEntityFrameworkAppRepository(MySqlOsekaiDbContext context)
    {
        Context = context;
    }

    public async Task<App?> GetAppByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        Entities.App? app = await Context.Apps.Include(a => a.AppTheme).FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
     
        if (app != null)
        {
            App appAggregate = app.ToAggregate();
            appAggregate.AppTheme = new Ref<AppTheme?>(app.AppTheme?.ToEntity());
            return appAggregate;
        }
        else
            return null;
    }

    public async Task<bool> SaveAppAsync(App app, CancellationToken cancellationToken = default)
    {
        Entities.App? appEntity = await Context.Apps.FindAsync(new object?[] { app.Id }, cancellationToken);
        if (appEntity == null)
            return false;
        
        appEntity.Name = app.Name;
        appEntity.SimpleName = app.SimpleName;
        appEntity.Order = app.Order;
        appEntity.Visible = app.Visible;
        appEntity.Experimental = app.Experimental;

        return true;
    }

    public async Task<IEnumerable<App>> GetAppsAsync(CancellationToken cancellationToken = default)
    {
        IEnumerable<Entities.App> apps = await Context.Apps.Include(app => app.AppTheme).ToArrayAsync(cancellationToken);
        return apps.Select(a =>
        {
            App appAggregate = a.ToAggregate();
            appAggregate.AppTheme = new Ref<AppTheme?>(a.AppTheme?.ToEntity());
            
            return appAggregate;
        });
    }
}