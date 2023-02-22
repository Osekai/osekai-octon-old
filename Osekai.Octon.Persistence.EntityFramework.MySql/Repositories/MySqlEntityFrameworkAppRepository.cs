using Microsoft.EntityFrameworkCore;
using Osekai.Octon.Domain.Aggregates;
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

    public async Task<App?> GetAppByIdAsync(int id, bool includeFaqs = false, CancellationToken cancellationToken = default)
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

    public async Task<IEnumerable<App>> GetAppsAsync(bool includeFaqs = false, CancellationToken cancellationToken = default)
    {
        IQueryable<Entities.App> query = Context.Apps.Include(app => app.AppTheme);
        if (includeFaqs)
            query.Include(app => app.Faqs);

        IEnumerable<Entities.App> apps = await query.ToArrayAsync(cancellationToken);
        IAsyncEnumerable<Entities.HomeFaq> faqs = Context.Faqs.ToAsyncEnumerable();

        var appsFaqs = await faqs.GroupBy(e => e.AppId, e => e)
            .ToDictionaryAwaitAsync(e => ValueTask.FromResult(e.Key), async e => await e.OrderBy(a => a.Id).ToArrayAsync());

        return apps.Select(a =>
        {
            App appAggregate = a.ToAggregate();
            appAggregate.AppTheme = new Ref<AppTheme?>(a.AppTheme?.ToEntity());

            if (includeFaqs)
            {
                if (appsFaqs.TryGetValue(appAggregate.Id, out Entities.HomeFaq[]? appFaqs))
                    appAggregate.Faqs = new Ref<IReadOnlyList<HomeFaq>>(appFaqs.Select(s => s.ToValueObject()).ToArray());
                else
                    appAggregate.Faqs = new Ref<IReadOnlyList<HomeFaq>>(Array.Empty<HomeFaq>());
            }
            
            return appAggregate;
        });
    }
}