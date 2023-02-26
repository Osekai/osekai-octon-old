using Osekai.Octon.Caching;
using Osekai.Octon.Domain.AggregateRoots;
using Osekai.Octon.Domain.Services;
using Osekai.Octon.OsuApi;
using Osekai.Octon.Domain.Services.Default;
using Osekai.Octon.WebServer.Presentation.AppBaseLayout;

namespace Osekai.Octon.WebServer.Pages;

public class Home : AppBaseLayout
{
    public override string MetadataDescription => "We're a website which provides osu! players with medal solutions, an alternative leaderboard, and much more!";
    public override string MetadataTitle => "Osekai • the home of alternative rankings, medal solutions, and more";
    public override string MetadataThemeColor => "#353d55";
    public override string MetadataUrl => "https://osekai.net/home";

    public Home(CurrentSession currentSession, CurrentLocale currentLocale, CachedAuthenticatedOsuApiV2Interface cachedAuthenticatedOsuApiV2Interface, IConverter<Medal, AppBaseLayoutMedal> appBaseLayoutMedalConverter, IConverter<UserGroup, AppBaseLayoutUserGroup> appBaseLayoutUserGroupConverter, IConverter<App, AppBaseLayoutApp> appBaseLayoutAppConverter, IAppService appService, IMedalService medalService, ICache cache, IUserGroupService userGroupService, ILocaleService localeService) : base(currentSession, currentLocale, cachedAuthenticatedOsuApiV2Interface, appBaseLayoutMedalConverter, appBaseLayoutUserGroupConverter, appBaseLayoutAppConverter, appService, medalService, cache, userGroupService, localeService)
    {
    }

    protected override int AppId => -1;
}