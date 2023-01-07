using System.Drawing;
using Microsoft.AspNetCore.Mvc;
using Osekai.Octon.DataAdapter;
using Osekai.Octon.Database;
using Osekai.Octon.Database.Dtos;
using Osekai.Octon.OsuApi;
using Osekai.Octon.OsuApi.Payloads;

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
    
    protected IDatabaseUnitOfWorkFactory DatabaseUnitOfWorkFactory { get; }
    protected CachedOsekaiDataAdapter OsekaiDataAdapter { get; }
    protected CachedAuthenticatedOsuApiV2Interface OsuApiV2Interface { get; }
    protected CurrentSession CurrentSession { get; }
    
    public virtual AccentOverride? AccentOvveride => null;
    public IEnumerable<OsekaiMedalData> Medals { get; private set; } = null!;

    public AppDto App { get; private set; } = null!;
    
    public OsuUser? CurrentOsuUser { get; private set; }
    
    protected AppBaseLayout(
        CurrentSession currentSession,
        CachedAuthenticatedOsuApiV2Interface cachedAuthenticatedOsuApiV2Interface, 
        CachedOsekaiDataAdapter osekaiDataAdapter, 
        IDatabaseUnitOfWorkFactory databaseUnitOfWorkFactory, 
        int appId)
    {
        CurrentSession = currentSession;
        OsuApiV2Interface = cachedAuthenticatedOsuApiV2Interface;
        DatabaseUnitOfWorkFactory = databaseUnitOfWorkFactory;
        OsekaiDataAdapter = osekaiDataAdapter;
        _appId = appId;
    }

    public virtual async Task<IActionResult> OnGet(CancellationToken cancellationToken)
    {
        await using IDatabaseUnitOfWork unitOfWork = await DatabaseUnitOfWorkFactory.CreateAsync(cancellationToken);
        
        App = await unitOfWork.AppRepository.GetAppByIdAsync(_appId, cancellationToken) ?? 
              throw new ArgumentException($"The application with Id {_appId} does not exist");

        if (App.AppTheme == null)
            throw new ArgumentException($"The application with Id {_appId} doesn't have a theme. It cannot be displayed");
        
        Medals = await OsekaiDataAdapter.GetMedalDataAsync(cancellationToken);
        
        if (!CurrentSession.IsNull())
            CurrentOsuUser = await OsuApiV2Interface.MeAsync(CurrentSession, cancellationToken: cancellationToken);
        
        return Page();
    }
}