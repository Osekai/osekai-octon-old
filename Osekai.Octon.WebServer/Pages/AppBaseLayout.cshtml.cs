using System.Drawing;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
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
    
    private int _appId;
    
    protected AppService AppService { get; }
    protected CachedAppBaseLayoutMedalDataGenerator AppBaseLayoutMedalDataGenerator { get; }
    protected IAppBaseLayoutUserGroupDataGenerator AppBaseLayoutUserGroupDataGenerator { get; }
    protected CachedAuthenticatedOsuApiV2Interface OsuApiV2Interface { get; }
    protected CurrentSession CurrentSession { get; }
    
    public virtual AccentOverride? AccentOvveride => null;
    public IEnumerable<AppBaseLayoutMedalData> Medals { get; private set; } = null!;
    public IEnumerable<AppBaseLayoutUserGroupData> UserGroups { get; private set; } = null!;

    public App App { get; private set; } = null!;

    public AppTheme AppTheme { get; private set; } = null!;
    
    public OsuUser? CurrentOsuUser { get; private set; }
    
    public bool? IsUserAdmin { get; private set; }
    
    protected AppBaseLayout(
        CurrentSession currentSession,
        CachedAuthenticatedOsuApiV2Interface cachedAuthenticatedOsuApiV2Interface, 
        CachedAppBaseLayoutMedalDataGenerator appBaseLayoutMedalDataGenerator, 
        IAppBaseLayoutUserGroupDataGenerator appBaseLayoutUserGroupDataGenerator,
        AppService appService,
        int appId)
    {
        CurrentSession = currentSession;
        OsuApiV2Interface = cachedAuthenticatedOsuApiV2Interface;
        AppService = appService;
        AppBaseLayoutMedalDataGenerator = appBaseLayoutMedalDataGenerator;
        AppBaseLayoutUserGroupDataGenerator = appBaseLayoutUserGroupDataGenerator;
        _appId = appId;
    }

    public virtual async Task<IActionResult> OnGet(CancellationToken cancellationToken)
    {
        App = await AppService.GetAppByIdAsync(_appId, cancellationToken) ?? 
              throw new ArgumentException($"The application with Id {_appId} does not exist");
        
        AppTheme = await App.GetAppThemeAsync(cancellationToken) ?? 
                   throw new ArgumentException($"The application with Id {_appId} doesn't have a theme. It cannot be displayed");

        Medals = await AppBaseLayoutMedalDataGenerator.GenerateAsync(cancellationToken);
        UserGroups = await AppBaseLayoutUserGroupDataGenerator.GenerateAsync(cancellationToken);

        if (!CurrentSession.IsNull())
        {
            CurrentOsuUser = await OsuApiV2Interface.MeAsync(CurrentSession, cancellationToken: cancellationToken);
            IsUserAdmin = await CurrentSession.PermissionStore!.HasPermissionAsync("admin");
        }

        return Page();
    }
}