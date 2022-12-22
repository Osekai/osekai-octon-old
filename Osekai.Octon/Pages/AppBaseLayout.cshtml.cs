using System.Drawing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Osekai.Octon.Database.EntityFramework;
using Osekai.Octon.Database.Repositories;

namespace Osekai.Octon.Pages;

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
    
    protected IAppRepository AppRepository { get; }

    public virtual AccentOverride? AccentOvveride => null;
    
    protected AppBaseLayout(IAppRepository appRepository, int appId)
    {
        AppRepository = appRepository;
        _appId = appId;
    }
    
    public virtual async Task<IActionResult> OnGet(CancellationToken cancellationToken)
    {
        App = await AppRepository.GetAppByIdAsync(_appId, includeTheme: true, cancellationToken) ?? 
              throw new ArgumentException($"The application with Id {_appId} does not exist");

        if (App.AppTheme == null)
            throw new ArgumentException($"The application with Id {_appId} doesn't have a theme. It cannot be displayed");
        
        return Page();
    }

    public App App { get; private set; } = null!;
}