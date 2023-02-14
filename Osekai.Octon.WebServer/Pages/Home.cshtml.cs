using Osekai.Octon.Models;
using Osekai.Octon.Persistence;
using Osekai.Octon.OsuApi;
using Osekai.Octon.Persistence.QueryResults;
using Osekai.Octon.Services;
using Osekai.Octon.WebServer.Presentation.AppBaseLayout;

namespace Osekai.Octon.WebServer.Pages;

public class Home : AppBaseLayout
{
    public override string MetadataDescription => "We're a website which provides osu! players with medal solutions, an alternative leaderboard, and much more!";
    public override string MetadataTitle => "Osekai • the home of alternative rankings, medal solutions, and more";
    public override string MetadataThemeColor => "#353d55";
    public override string MetadataUrl => "https://osekai.net/home";

    public Home(CurrentSession currentSession, CurrentLocale currentLocale, CachedAuthenticatedOsuApiV2Interface cachedAuthenticatedOsuApiV2Interface, IAdapter<IReadOnlyMedalAggregateQueryResult, AppBaseLayoutMedal> appBaseLayoutMedalAdapter, IAdapter<IReadOnlyUserGroup, AppBaseLayoutUserGroup> appBaseLayoutUserGroupAdapter, IAdapter<IReadOnlyAppAggregateQueryResult, AppBaseLayoutApp> appBaseLayoutAppAdapter, IQuery<IReadOnlyAppAggregateQueryResult> appAggregateQuery, IQuery<IReadOnlyMedalAggregateQueryResult> medalAggregateQuery, ICache cache, UserGroupService userGroupService, LocaleService localeService) 
        : base(currentSession, currentLocale, cachedAuthenticatedOsuApiV2Interface, appBaseLayoutMedalAdapter, appBaseLayoutUserGroupAdapter, appBaseLayoutAppAdapter, appAggregateQuery, medalAggregateQuery, cache, userGroupService, localeService, -1)
    {
    }
}