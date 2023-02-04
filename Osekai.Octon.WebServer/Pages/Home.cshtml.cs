using Osekai.Octon.Objects;
using Osekai.Octon.Objects.Aggregators;
using Osekai.Octon.Persistence;
using Osekai.Octon.OsuApi;
using Osekai.Octon.Services;
using Osekai.Octon.WebServer.Presentation.AppBaseLayout;

namespace Osekai.Octon.WebServer.Pages;

public class Home : AppBaseLayout
{
    public Home(CurrentSession currentSession, CurrentLocale currentLocale, CachedAuthenticatedOsuApiV2Interface cachedAuthenticatedOsuApiV2Interface, IAdapter<IReadOnlyMedalWithInfo, AppBaseLayoutMedal> appBaseLayoutMedalAdapter, IAdapter<IReadOnlyUserGroup, AppBaseLayoutUserGroup> appBaseLayoutUserGroupAdapter, IAdapter<IReadOnlyAppWithAppTheme, AppBaseLayoutApp> appBaseLayoutAppAdapter, IAggregator<IReadOnlyAppWithAppTheme> appAggregator, IAggregator<IReadOnlyMedalWithInfo> medalAggregator, ICache cache, UserGroupService userGroupService) 
        : base(currentSession, currentLocale, cachedAuthenticatedOsuApiV2Interface, appBaseLayoutMedalAdapter, appBaseLayoutUserGroupAdapter, appBaseLayoutAppAdapter, appAggregator, medalAggregator, cache, userGroupService, -1)
    {
    }

    public override string MetadataDescription => "We're a website which provides osu! players with medal solutions, an alternative leaderboard, and much more!";
    public override string MetadataTitle => "Osekai • the home of alternative rankings, medal solutions, and more";
    public override string MetadataThemeColor => "#353d55";
    public override string MetadataUrl => "https://osekai.net/home";
}