using System.Drawing;
using Microsoft.AspNetCore.Mvc;
using Osekai.Octon.Localization;
using Osekai.Octon.Objects;
using Osekai.Octon.Objects.Aggregators;
using Osekai.Octon.Persistence;
using Osekai.Octon.OsuApi;
using Osekai.Octon.OsuApi.Payloads;
using Osekai.Octon.Services;
using Osekai.Octon.Services.Entities;
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
    protected IAdapter<IReadOnlyMedalWithInfo, AppBaseLayoutMedal> AppBaseLayoutMedalAdapter { get; }
    protected IAdapter<IReadOnlyAppWithAppTheme, AppBaseLayoutApp> AppBaseLayoutAppAdapter { get; }
    protected CachedAuthenticatedOsuApiV2Interface OsuApiV2Interface { get; }
    protected CurrentSession CurrentSession { get; }
    protected IAggregator<IReadOnlyAppWithAppTheme> AppAggregator { get; }
    protected IAggregator<IReadOnlyMedalWithInfo> MedalAggregator { get; }
    protected UserGroupService UserGroupService { get; }
    protected ICache Cache { get; }

    public virtual AccentOverride? AccentOvveride => null;
    public IReadOnlyCollection<AppBaseLayoutMedal> AppBaseLayoutMedals { get; private set; } = null!;
    public IReadOnlyCollection<AppBaseLayoutUserGroup> AppBaseLayoutUserGroups { get; private set; } = null!;
    public IReadOnlyDictionary<string, AppBaseLayoutApp> AppBaseLayoutApps { get; private set; } = null!;
    public CurrentLocale CurrentLocale { get; }
    public IReadOnlyDictionary<int, IReadOnlyAppWithAppTheme> Apps { get; private set; } = null!;
    public IReadOnlyCollection<IReadOnlyUserGroup> UserGroups { get; private set; } = null!;
    public IReadOnlyCollection<IReadOnlyMedalWithInfo> Medals { get; private set; } = null!;
    public IReadOnlyApp CurrentApp { get; private set; } = null!;
    public IReadOnlyAppTheme CurrentAppTheme { get; private set; } = null!;
    public OsuUser? CurrentOsuUser { get; private set; }

    public virtual bool ShowNavbar => true;

    protected AppBaseLayout(
        CurrentSession currentSession,
        CurrentLocale currentLocale,
        CachedAuthenticatedOsuApiV2Interface cachedAuthenticatedOsuApiV2Interface, 
        IAdapter<IReadOnlyMedalWithInfo, AppBaseLayoutMedal> appBaseLayoutMedalAdapter, 
        IAdapter<IReadOnlyUserGroup, AppBaseLayoutUserGroup> appBaseLayoutUserGroupAdapter,
        IAdapter<IReadOnlyAppWithAppTheme, AppBaseLayoutApp> appBaseLayoutAppAdapter,
        IAggregator<IReadOnlyAppWithAppTheme> appAggregator,
        IAggregator<IReadOnlyMedalWithInfo> medalAggregator,
        ICache cache,
        UserGroupService userGroupService,
        int appId)
    {
        CurrentSession = currentSession;
        OsuApiV2Interface = cachedAuthenticatedOsuApiV2Interface;
        AppAggregator = appAggregator;
        MedalAggregator = medalAggregator;
        AppBaseLayoutMedalAdapter = appBaseLayoutMedalAdapter;
        AppBaseLayoutUserGroupAdapter = appBaseLayoutUserGroupAdapter;
        AppBaseLayoutAppAdapter = appBaseLayoutAppAdapter;
        CurrentLocale = currentLocale;
        UserGroupService = userGroupService;
        AppId = appId;
        Cache = cache;
    }

    private class AppBaseLayoutCacheEntry
    {
        public IReadOnlyDictionary<int, IReadOnlyAppWithAppTheme> Apps { get; init; } = null!;
        public IReadOnlyCollection<IReadOnlyMedalWithInfo> Medals { get; init; } = null!;
        public IReadOnlyCollection<IReadOnlyUserGroup> UserGroups { get; init; } = null!;
        public IReadOnlyDictionary<string, AppBaseLayoutApp> AppBaseLayoutApps { get; init; } = null!;
        public IReadOnlyCollection<AppBaseLayoutUserGroup> AppBaseLayoutUserGroups { get; init; } = null!;
        public IReadOnlyCollection<AppBaseLayoutMedal> AppBaseLayoutMedals { get; init; } = null!;
    }

    public virtual async Task<IActionResult> OnGet(CancellationToken cancellationToken)
    {
        AppBaseLayoutCacheEntry? appBaseLayoutCacheEntry = await Cache.GetAsync<AppBaseLayoutCacheEntry>("app_base_layout_cache_entry");

        if (appBaseLayoutCacheEntry == null)
        {
            Apps = (await AppAggregator.AggregateAllAsync(cancellationToken)).ToDictionary(k => k.App.Id, e => e);
            Medals = (await MedalAggregator.AggregateAllAsync(cancellationToken)).ToArray();
            UserGroups = (await UserGroupService.GetUserGroupsAsync(cancellationToken)).ToArray();
            
            AppBaseLayoutApps = await Apps.ToAsyncEnumerable().ToDictionaryAwaitAsync(
                k => ValueTask.FromResult(k.Value.App.SimpleName),
                async v => await AppBaseLayoutAppAdapter.AdaptAsync(v.Value, cancellationToken));

            AppBaseLayoutMedals = await Medals.ToAsyncEnumerable()
                .SelectAwait(async v => await AppBaseLayoutMedalAdapter.AdaptAsync(v))
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
                AppBaseLayoutUserGroups = AppBaseLayoutUserGroups
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
        }

        if (!Apps.TryGetValue(AppId, out IReadOnlyAppWithAppTheme? app))
            throw new ArgumentException($"The App with Id {AppId} was not found");
        
        CurrentApp = app.App;
        CurrentAppTheme = app.AppTheme ?? throw new ArgumentException($"The App with Id {AppId} does not have a theme");
        
        if (!CurrentSession.IsNull())
            CurrentOsuUser = await OsuApiV2Interface.MeAsync(CurrentSession, cancellationToken: cancellationToken);

        return Page();
    }
}