using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using Osekai.Octon.Database.EntityFramework;
using Osekai.Octon.Database.Repositories;

namespace Osekai.Octon.Pages;

public class Home : AppBaseLayout
{
    public override string MetadataDescription => "We're a website which provides osu! players with medal solutions, an alternative leaderboard, and much more!";
    public override string MetadataTitle => "Osekai • the home of alternative rankings, medal solutions, and more";
    public override string MetadataThemeColor => "#353d55";
    public override string MetadataUrl => "https://osekai.net/home";
    public Home(IAppRepository appRepository) : base(appRepository, appId: -1)  {}
}