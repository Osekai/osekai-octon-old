using Microsoft.AspNetCore.Mvc.RazorPages;
using Osekai.Octon.Localization;
using Osekai.Octon.Models;
using Osekai.Octon.OsuApi.Payloads;
using Osekai.Octon.Persistence.QueryResults;
using Osekai.Octon.WebServer.Presentation.AppBaseLayout;

namespace Osekai.Octon.WebServer.Pages.Partials;

public class NavbarPartial: PageModel
{
    public ILocalizator Localizator { get; }
    public IReadOnlyApp App { get; }
    public IReadOnlyDictionary<int, IReadOnlyAppAggregateQueryResult> Apps { get; }
    public IReadOnlyDictionary<string, AppBaseLayoutApp> AppBaseLayoutApps { get; }
    public OsuUser? AuthenticatedUser { get; }

    public NavbarPartial(
        ILocalizator localizator, 
        IReadOnlyApp app, 
        IReadOnlyDictionary<int, IReadOnlyAppAggregateQueryResult> apps,
        IReadOnlyDictionary<string, AppBaseLayoutApp> appBaseLayoutApps, 
        OsuUser? authenticatedUser)
    {
        Localizator = localizator;
        App = app;
        Apps = apps;
        AppBaseLayoutApps = appBaseLayoutApps;
        AuthenticatedUser = authenticatedUser;
    }
}
