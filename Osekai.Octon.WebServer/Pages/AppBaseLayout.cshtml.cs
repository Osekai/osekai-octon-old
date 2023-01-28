using System.Drawing;
using Microsoft.AspNetCore.Mvc;
using Osekai.Octon.Persistence;
using Osekai.Octon.OsuApi;
using Osekai.Octon.OsuApi.Payloads;
using Osekai.Octon.Services;
using Osekai.Octon.Services.Entities;
using Osekai.Octon.WebServer.API.V1.DataAdapter;

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
    protected CachedOsekaiMedalDataGenerator OsekaiMedalDataGenerator { get; }
    protected CachedAuthenticatedOsuApiV2Interface OsuApiV2Interface { get; }
    protected CurrentSession CurrentSession { get; }
    
    public virtual AccentOverride? AccentOvveride => null;
    public IEnumerable<OsekaiMedalData> Medals { get; private set; } = null!;

    public App App { get; private set; } = null!;

    public AppTheme AppTheme { get; private set; } = null!;
    
    public OsuUser? CurrentOsuUser { get; private set; }
    
    protected AppBaseLayout(
        CurrentSession currentSession,
        CachedAuthenticatedOsuApiV2Interface cachedAuthenticatedOsuApiV2Interface, 
        CachedOsekaiMedalDataGenerator osekaiMedalDataGenerator, 
        AppService appService,
        int appId)
    {
        CurrentSession = currentSession;
        OsuApiV2Interface = cachedAuthenticatedOsuApiV2Interface;
        AppService = appService;
        OsekaiMedalDataGenerator = osekaiMedalDataGenerator;
        _appId = appId;
    }

    public virtual async Task<IActionResult> OnGet(CancellationToken cancellationToken)
    {
        App = await AppService.GetAppByIdAsync(_appId, cancellationToken) ?? 
              throw new ArgumentException($"The application with Id {_appId} does not exist");
        
        AppTheme = await App.GetAppThemeAsync(cancellationToken) ?? 
                   throw new ArgumentException($"The application with Id {_appId} doesn't have a theme. It cannot be displayed");

        Medals = await OsekaiMedalDataGenerator.GetOsekaiMedalDataAsync(cancellationToken);
        
        if (!CurrentSession.IsNull())
            CurrentOsuUser = await OsuApiV2Interface.MeAsync(CurrentSession, cancellationToken: cancellationToken);
        
        return Page();
    }
}