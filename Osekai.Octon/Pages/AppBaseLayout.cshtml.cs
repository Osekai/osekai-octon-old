using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Osekai.Octon.Database.EntityFramework;
using Osekai.Octon.Database.Repositories;

namespace Osekai.Octon.Pages;

public abstract class AppBaseLayout : BaseLayout
{
    private int _appId;
    
    protected IAppRepository AppRepository { get; }

    protected AppBaseLayout(IAppRepository appRepository, int appId)
    {
        AppRepository = appRepository;
        _appId = appId;
    }
    
    public virtual async Task<IActionResult> OnGet()
    {
        App = await AppRepository.GetAppByIdAsync(_appId) ?? 
              throw new ArgumentException($"The application with Id {_appId} does not exist.");

        return Page();
    }

    public App App { get; private set; } = null!;
}