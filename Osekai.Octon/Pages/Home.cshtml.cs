using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;

namespace Osekai.Octon.Pages;

public class Home : BaseLayout
{
    public IActionResult OnGet()
    {
        return Page();
    }
    
    public override string MetadataDescription => "We're a website which provides osu! players with medal solutions, an alternative leaderboard, and much more!";
    public override string MetadataTitle => "Osekai • the home of alternative rankings, medal solutions, and more";
    public override string MetadataThemeColor => "#353d55";
    public override string MetadataUrl => "";
    
    public override IReadOnlyCollection<string> MetadataKeywords => 
        new string[] { "osekai", "medals","osu","achievements","rankings","alternative","medal rankings","osekai","the","home","of","more" };
}