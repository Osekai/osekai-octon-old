using Microsoft.AspNetCore.Mvc;
using Osekai.Octon.Caching;
using Osekai.Octon.Domain.AggregateRoots;
using Osekai.Octon.Domain.Services;
using Osekai.Octon.Domain.ValueObjects;
using Osekai.Octon.Drawing;
using Osekai.Octon.Extensions;
using Osekai.Octon.OsuApi;
using Osekai.Octon.OsuApi.Payloads;
using Osekai.Octon.WebServer.Presentation.AppBaseLayout;

namespace Osekai.Octon.WebServer.Pages;

[DefaultAuthenticationFilter]
public abstract class AppBaseLayout : BaseLayout
{
    public readonly struct AccentOverride
    {
        public AccentOverride(RgbColour color, RgbColour darkColor)
        {
            Color = color;
            DarkColor = darkColor;
        }

        public RgbColour Color { get; }
        public RgbColour DarkColor { get; }
    } 
    
    protected abstract int AppId { get; }
    protected IConverter<UserGroup, AppBaseLayoutUserGroup> AppBaseLayoutUserGroupConverter { get; }
    protected IConverter<Medal, AppBaseLayoutMedal> AppBaseLayoutMedalConverter { get; }
    protected IConverter<App, AppBaseLayoutApp> AppBaseLayoutAppConverter { get; }
    protected CachedAuthenticatedOsuApiV2Interface OsuApiV2Interface { get; }
    protected CurrentSession CurrentSession { get; }
    protected IUserGroupService UserGroupService { get; }
    protected IMedalService MedalService { get; }
    protected IAppService AppService { get; }
    protected ILocaleService LocaleService { get; }
    protected ICache Cache { get; }
    
    public virtual AccentOverride? AccentOvveride => null;
    public IReadOnlyCollection<AppBaseLayoutMedal> AppBaseLayoutMedals { get; private set; } = null!;
    public IReadOnlyCollection<AppBaseLayoutUserGroup> AppBaseLayoutUserGroups { get; private set; } = null!;
    public IReadOnlyDictionary<string, AppBaseLayoutApp> AppBaseLayoutApps { get; private set; } = null!;
    public IReadOnlyDictionary<string, AppBaseLayoutLocale> AppBaseLayoutLocales { get; private set; } = null!;
    public CurrentLocale CurrentLocale { get; }
    public IReadOnlyDictionary<int, App> Apps { get; private set; } = null!;
    public IReadOnlyCollection<UserGroup> UserGroups { get; private set; } = null!;
    public IReadOnlyCollection<Medal> Medals { get; private set; } = null!;
    public App CurrentApp { get; private set; } = null!;
    public AppTheme CurrentAppTheme { get; private set; } 
    public OsuUser? CurrentOsuUser { get; private set; }
    public bool Mobile { get; private set; }
    public bool Experimental { get; private set; }
    
    public virtual bool ShowNavbar => true;

    protected AppBaseLayout(
        CurrentSession currentSession,
        CurrentLocale currentLocale,
        CachedAuthenticatedOsuApiV2Interface cachedAuthenticatedOsuApiV2Interface, 
        IConverter<Medal, AppBaseLayoutMedal> appBaseLayoutMedalConverter, 
        IConverter<UserGroup, AppBaseLayoutUserGroup> appBaseLayoutUserGroupConverter,
        IConverter<App, AppBaseLayoutApp> appBaseLayoutAppConverter,
        IAppService appService,
        IMedalService medalService,
        ICache cache,
        IUserGroupService userGroupService,
        ILocaleService localeService)
    {
        CurrentSession = currentSession;
        OsuApiV2Interface = cachedAuthenticatedOsuApiV2Interface;
        AppBaseLayoutMedalConverter = appBaseLayoutMedalConverter;
        AppBaseLayoutUserGroupConverter = appBaseLayoutUserGroupConverter;
        AppBaseLayoutAppConverter = appBaseLayoutAppConverter;
        CurrentLocale = currentLocale;
        UserGroupService = userGroupService;
        LocaleService = localeService;
        AppService = appService;
        MedalService = medalService;
        Cache = cache;
    }

    public class AppBaseLayoutCacheEntry
    {
        public IReadOnlyDictionary<int, App> Apps { get; init; } = null!;
        public IReadOnlyCollection<Medal> Medals { get; init; } = null!;
        public IReadOnlyCollection<UserGroup> UserGroups { get; init; } = null!;
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
            Apps = (await AppService.GetAppsAsync(cancellationToken: cancellationToken)).ToDictionary(k => k.Id, e => e);
            Medals = (await MedalService.GetMedalsAsync(includeBeatmapPacks: true, cancellationToken: cancellationToken)).ToArray();
            UserGroups = (await UserGroupService.GetUserGroupsAsync(cancellationToken)).ToArray();

            AppBaseLayoutLocales = (await LocaleService.GetLocalesAsync(cancellationToken))
                .Select(locale => new AppBaseLayoutLocale(locale.Name, locale.Code, locale.ShortName, locale.Flag.ToString(),
                    locale.Experimental, locale.Wip, locale.Rtl, locale.ExtraHtml, locale.ExtraCss))
                .ToDictionary(l => l.Code, l => l);
            
            AppBaseLayoutApps = await Apps.ToAsyncEnumerable().ToDictionaryAwaitAsync(
                k => ValueTask.FromResult(k.Value.SimpleName),
                async v => await AppBaseLayoutAppConverter.ConvertAsync(v.Value, cancellationToken));

            AppBaseLayoutMedals = await Medals.ToAsyncEnumerable()
                .SelectAwait(async v => await AppBaseLayoutMedalConverter.ConvertAsync(v, cancellationToken))
                .ToArrayAsync(cancellationToken);

            AppBaseLayoutUserGroups = await UserGroups.ToAsyncEnumerable()
                .SelectAwait(async v => await AppBaseLayoutUserGroupConverter.ConvertAsync(v, cancellationToken))
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
            }, cancellationToken: cancellationToken);
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

        if (!Apps.TryGetValue(AppId, out App? app))
            throw new ArgumentException($"The App with Id {AppId} was not found");
        
        CurrentApp = app;
        CurrentAppTheme = app.AppTheme.Value ?? throw new ArgumentException($"The App with Id {AppId} does not have a theme");
        
        if (!CurrentSession.IsNull())
            CurrentOsuUser = await OsuApiV2Interface.MeAsync(CurrentSession, cancellationToken: cancellationToken);

        if (MobileUserAgentMatcher.IsMatch(HttpContext.Request.Headers.UserAgent.ToString()))
            Mobile = true;

        Experimental = true;
        
        return Page();
    }
}