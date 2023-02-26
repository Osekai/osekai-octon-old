using Microsoft.EntityFrameworkCore;
using Osekai.Octon.Domain.Repositories;
using Osekai.Octon.Domain.ValueObjects;
using App = Osekai.Octon.Domain.AggregateRoots.App;

namespace Osekai.Octon.Persistence.EntityFramework.MySql.Repositories;

public class MySqlEntityFrameworkAppRepository: IAppRepository
{
    protected MySqlOsekaiDbContext Context { get; }
    internal MySqlEntityFrameworkAppRepository(MySqlOsekaiDbContext context)
    {
        Context = context;
    }

    public async Task<App?> GetAppByIdAsync(int id, bool includeFaqs = false, CancellationToken cancellationToken = default)
    {
        Entities.App? app = await Context.Apps.Include(a => a.AppTheme).FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
     
        if (app != null)
        {
            App appAggregate = app.ToAggregateRoot();
            appAggregate.AppTheme = app.AppTheme?.ToValueObject();
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

    public async Task<IEnumerable<App>> GetAppsAsync(bool includeFaqs = false, CancellationToken cancellationToken = default)
    {
        IQueryable<Entities.App> query = Context.Apps.Include(app => app.AppTheme);
        if (includeFaqs)
            query.Include(app => app.Faqs);

        IEnumerable<Entities.App> apps = await query.ToArrayAsync(cancellationToken);
        Dictionary<int, Entities.HomeFaq[]> appsFaqs = await Context.Faqs.GroupBy(e => e.AppId).ToDictionaryAsync(e => e.Key, e => e.ToArray(), cancellationToken);
        
        return apps.Select(a =>
        {
            App appAggregate = a.ToAggregateRoot();
            appAggregate.AppTheme = a.AppTheme?.ToValueObject();

            if (includeFaqs)
            {
                if (appsFaqs.TryGetValue(appAggregate.Id, out Entities.HomeFaq[]? appFaqs))
                    appAggregate.Faqs = appFaqs.Select(s => s.ToValueObject()).ToArray();
                else
                    appAggregate.Faqs = Array.Empty<HomeFaq>();
            }
            
            return appAggregate;
        });
    }
}