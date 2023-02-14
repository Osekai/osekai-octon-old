using System.Drawing;
using Microsoft.AspNetCore.Mvc;
using Osekai.Octon.Localization;
using Osekai.Octon.Models;
using Osekai.Octon.Persistence;
using Osekai.Octon.OsuApi;
using Osekai.Octon.OsuApi.Payloads;
using Osekai.Octon.Persistence.EntityFramework.MySql.Entities;
using Osekai.Octon.Persistence.QueryResults;
using Osekai.Octon.Services;
using Osekai.Octon.WebServer.Presentation.AppBaseLayout;

namespace Osekai.Octon.WebServer.Pages;

[DefaultAuthenticationFilter]
public abstract class AppBaseLayout : BaseLayout
{
    public readonly struct AccentOverride
    {
        public AccentOverride(Color color, Color darkColor)
        {
            Color = color;
            DarkColor = darkColor;
        }

        public Color Color { get; }
        public Color DarkColor { get; }
    } 
    
    protected int AppId { get; }
    protected IAdapter<IReadOnlyUserGroup, AppBaseLayoutUserGroup> AppBaseLayoutUserGroupAdapter { get; }
    protected IAdapter<IReadOnlyMedalAggregateQueryResult, AppBaseLayoutMedal> AppBaseLayoutMedalAdapter { get; }
    protected IAdapter<IReadOnlyAppAggregateQueryResult, AppBaseLayoutApp> AppBaseLayoutAppAdapter { get; }
    protected CachedAuthenticatedOsuApiV2Interface OsuApiV2Interface { get; }
    protected CurrentSession CurrentSession { get; }
    protected IQuery<IReadOnlyAppAggregateQueryResult> AppQuery { get; }
    protected IQuery<IReadOnlyMedalAggregateQueryResult> MedalQuery { get; }
    protected UserGroupService UserGroupService { get; }
    protected LocaleService LocaleService { get; }
    protected ICache Cache { get; }

    public virtual AccentOverride? AccentOvveride => null;
    public IReadOnlyCollection<AppBaseLayoutMedal> AppBaseLayoutMedals { get; private set; } = null!;
    public IReadOnlyCollection<AppBaseLayoutUserGroup> AppBaseLayoutUserGroups { get; private set; } = null!;
    public IReadOnlyDictionary<string, AppBaseLayoutApp> AppBaseLayoutApps { get; private set; } = null!;
    public IReadOnlyDictionary<string, AppBaseLayoutLocale> AppBaseLayoutLocales { get; private set; } = null!;
    public CurrentLocale CurrentLocale { get; }
    public IReadOnlyDictionary<int, IReadOnlyAppAggregateQueryResult> Apps { get; private set; } = null!;
    public IReadOnlyCollection<IReadOnlyUserGroup> UserGroups { get; private set; } = null!;
    public IReadOnlyCollection<IReadOnlyMedalAggregateQueryResult> Medals { get; private set; } = null!;
    public IReadOnlyApp CurrentApp { get; private set; } = null!;
    public IReadOnlyAppTheme CurrentReadOnlyAppTheme { get; private set; } = null!;
    public OsuUser? CurrentOsuUser { get; private set; }

    public virtual bool ShowNavbar => true;

    protected AppBaseLayout(
        CurrentSession currentSession,
        CurrentLocale currentLocale,
        CachedAuthenticatedOsuApiV2Interface cachedAuthenticatedOsuApiV2Interface, 
        IAdapter<IReadOnlyMedalAggregateQueryResult, AppBaseLayoutMedal> appBaseLayoutMedalAdapter, 
        IAdapter<IReadOnlyUserGroup, AppBaseLayoutUserGroup> appBaseLayoutUserGroupAdapter,
        IAdapter<IReadOnlyAppAggregateQueryResult, AppBaseLayoutApp> appBaseLayoutAppAdapter,
        IQuery<IReadOnlyAppAggregateQueryResult> appQuery,
        IQuery<IReadOnlyMedalAggregateQueryResult> medalQuery,
        ICache cache,
        UserGroupService userGroupService,
        LocaleService localeService,
        int appId)
    {
        CurrentSession = currentSession;
        OsuApiV2Interface = cachedAuthenticatedOsuApiV2Interface;
        AppQuery = appQuery;
        MedalQuery = medalQuery;
        AppBaseLayoutMedalAdapter = appBaseLayoutMedalAdapter;
        AppBaseLayoutUserGroupAdapter = appBaseLayoutUserGroupAdapter;
        AppBaseLayoutAppAdapter = appBaseLayoutAppAdapter;
        CurrentLocale = currentLocale;
        UserGroupService = userGroupService;
        LocaleService = localeService;
        AppId = appId;
        Cache = cache;
    }

    private class AppBaseLayoutCacheEntry
    {
        public IReadOnlyDictionary<int, IReadOnlyAppAggregateQueryResult> Apps { get; init; } = null!;
        public IReadOnlyCollection<IReadOnlyMedalAggregateQueryResult> Medals { get; init; } = null!;
        public IReadOnlyCollection<IReadOnlyUserGroup> UserGroups { get; init; } = null!;
        public IReadOnlyDictionary<string, AppBaseLayoutApp> AppBaseLayoutApps { get; init; } = null!;
        public IReadOnlyCollection<AppBaseLayoutUserGroup> AppBaseLayoutUserGroups { get; init; } = null!;
        public IReadOnlyCollection<AppBaseLayoutMedal> AppBaseLayoutMedals { get; init; } = null!;
        public IReadOnlyDictionary<string, AppBaseLayoutLocale> AppBaseLayoutLocales { get; init; } = null!;
    }

    public virtual async Task<IActionResult> OnGet(CancellationToken cancellationToken)
    {
        AppBaseLayoutCacheEntry? appBaseLayoutCacheEntry = await Cache.GetAsync<AppBaseLayoutCacheEntry>("app_base_layout_cache_entry", cancellationToken);

        if (appBaseLayoutCacheEntry == null)
        {
            Apps = (await AppQuery.ExecuteAsync(cancellationToken)).ToDictionary(k => k.App.Id, e => e);
            Medals = (await MedalQuery.ExecuteAsync(cancellationToken)).ToArray();
            UserGroups = (await UserGroupService.GetUserGroupsAsync(cancellationToken)).ToArray();

            AppBaseLayoutLocales = (await LocaleService.GetLocalesAsync(cancellationToken))
                .Select(locale => new AppBaseLayoutLocale(locale.Name, locale.Code, locale.Short, locale.Flag.ToString(),
                    locale.Experimental, locale.Wip, locale.Rtl, locale.ExtraHtml, locale.ExtraCss))
                .ToDictionary(l => l.Code, l => l);
            
            AppBaseLayoutApps = await Apps.ToAsyncEnumerable().ToDictionaryAwaitAsync(
                k => ValueTask.FromResult(k.Value.App.SimpleName),
                async v => await AppBaseLayoutAppAdapter.AdaptAsync(v.Value, cancellationToken));

            AppBaseLayoutMedals = await Medals.ToAsyncEnumerable()
                .SelectAwait(async v => await AppBaseLayoutMedalAdapter.AdaptAsync(v, cancellationToken))
                .ToArrayAsync(cancellationToken);

            AppBaseLayoutUserGroups = await UserGroups.ToAsyncEnumerable()
                .SelectAwait(async v => await AppBaseLayoutUserGroupAdapter.AdaptAsync(v, cancellationToken))
                .ToArrayAsync(cancellationToken);
            
            await Cache.SetAsync("app_base_layout_cache_entry", new AppBaseLayoutCacheEntry
            {
                Apps = Apps,
                Medals = Medals,
                UserGroups = UserGroups,
                AppBaseLayoutApps = AppBaseLayoutApps,
                AppBaseLayoutMedals = AppBaseLayoutMedals,
                AppBaseLayoutUserGroups = AppBaseLayoutUserGroups,
                AppBaseLayoutLocales = AppBaseLayoutLocales
            });
        }
        else
        {
            Apps = appBaseLayoutCacheEntry.Apps;
            Medals = appBaseLayoutCacheEntry.Medals;
            UserGroups = appBaseLayoutCacheEntry.UserGroups;
            AppBaseLayoutApps = appBaseLayoutCacheEntry.AppBaseLayoutApps;
            AppBaseLayoutMedals = appBaseLayoutCacheEntry.AppBaseLayoutMedals;
            AppBaseLayoutUserGroups = appBaseLayoutCacheEntry.AppBaseLayoutUserGroups;
            AppBaseLayoutLocales = appBaseLayoutCacheEntry.AppBaseLayoutLocales;
        }

        if (!Apps.TryGetValue(AppId, out IReadOnlyAppAggregateQueryResult? app))
            throw new ArgumentException($"The App with Id {AppId} was not found");
        
        CurrentApp = app.App;
        CurrentReadOnlyAppTheme = app.AppTheme ?? throw new ArgumentException($"The App with Id {AppId} does not have a theme");
        
        if (!CurrentSession.IsNull())
            CurrentOsuUser = await OsuApiV2Interface.MeAsync(CurrentSession, cancellationToken: cancellationToken);

        return Page();
    }
}