using Microsoft.AspNetCore.Mvc.RazorPages;
using Osekai.Octon.Domain.Aggregates;
using Osekai.Octon.Localization;
using Osekai.Octon.OsuApi.Payloads;
using Osekai.Octon.WebServer.Presentation.AppBaseLayout;

namespace Osekai.Octon.WebServer.Pages.Partials;

public class NavbarPartial: PageModel
{
    public ILocalizator Localizator { get; }
    public App App { get; }
    public IReadOnlyDictionary<int, App> Apps { get; }
    public IReadOnlyDictionary<string, AppBaseLayoutApp> AppBaseLayoutApps { get; }
    public OsuUser? AuthenticatedUser { get; }
    public bool Experimental { get; }

    public NavbarPartial(
        ILocalizator localizator, 
        App app, 
        IReadOnlyDictionary<int, App> apps,
        IReadOnlyDictionary<string, AppBaseLayoutApp> appBaseLayoutApps,
        OsuUser? authenticatedUser,
        bool experimental)
    {
        Localizator = localizator;
        App = app;
        Apps = apps;
        AppBaseLayoutApps = appBaseLayoutApps;
        AuthenticatedUser = authenticatedUser;
        Experimental = experimental;
    }
}
