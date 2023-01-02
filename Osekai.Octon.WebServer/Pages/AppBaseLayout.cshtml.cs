using System.Drawing;
using Microsoft.AspNetCore.Mvc;
using Osekai.Octon.Database;
using Osekai.Octon.Database.Dtos;

namespace Osekai.Octon.WebServer.Pages;

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

    public virtual AccentOverride? AccentOvveride => null;
    
    protected AppBaseLayout(IDatabaseUnitOfWorkFactory databaseUnitOfWorkFactory, int appId)
    {
        DatabaseUnitOfWorkFactory = databaseUnitOfWorkFactory;
        _appId = appId;
    }
    
    public virtual async Task<IActionResult> OnGet(CancellationToken cancellationToken)
    {
        IDatabaseUnitOfWork unitOfWork = await DatabaseUnitOfWorkFactory.CreateAsync();
        
        App = await unitOfWork.AppRepository.GetAppByIdAsync(_appId, cancellationToken) ?? 
              throw new ArgumentException($"The application with Id {_appId} does not exist");

        if (App.AppTheme == null)
            throw new ArgumentException($"The application with Id {_appId} doesn't have a theme. It cannot be displayed");

        return Page();
    }

    public AppDto App { get; private set; } = null!;
}