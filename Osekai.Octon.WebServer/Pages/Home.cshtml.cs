using Osekai.Octon.Caching;
using Osekai.Octon.Domain.AggregateRoots;
using Osekai.Octon.OsuApi;
using Osekai.Octon.Services;
using Osekai.Octon.WebServer.Presentation.AppBaseLayout;

namespace Osekai.Octon.WebServer.Pages;

public class Home : AppBaseLayout
{
    public override string MetadataDescription => "We're a website which provides osu! players with medal solutions, an alternative leaderboard, and much more!";
    public override string MetadataTitle => "Osekai • the home of alternative rankings, medal solutions, and more";
    public override string MetadataThemeColor => "#353d55";
    public override string MetadataUrl => "https://osekai.net/home";

    public Home(CurrentSession currentSession, CurrentLocale currentLocale, CachedAuthenticatedOsuApiV2Interface cachedAuthenticatedOsuApiV2Interface, IAdapter<Medal, AppBaseLayoutMedal> appBaseLayoutMedalAdapter, IAdapter<UserGroup, AppBaseLayoutUserGroup> appBaseLayoutUserGroupAdapter, IAdapter<App, AppBaseLayoutApp> appBaseLayoutAppAdapter, AppService appService, MedalService medalService, ICache cache, UserGroupService userGroupService, LocaleService localeService) 
        : base(currentSession, currentLocale, cachedAuthenticatedOsuApiV2Interface, appBaseLayoutMedalAdapter, appBaseLayoutUserGroupAdapter, appBaseLayoutAppAdapter, appService, medalService, cache, userGroupService, localeService, -1)
    {
    }
}