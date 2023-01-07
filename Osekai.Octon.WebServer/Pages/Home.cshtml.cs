using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using Osekai.Octon.DataAdapter;
using Osekai.Octon.Database;
using Osekai.Octon.Database.EntityFramework;
using Osekai.Octon.Database.Repositories;
using Osekai.Octon.OsuApi;

namespace Osekai.Octon.WebServer.Pages;

public class Home : AppBaseLayout
{
    public override string MetadataDescription => "We're a website which provides osu! players with medal solutions, an alternative leaderboard, and much more!";
    public override string MetadataTitle => "Osekai • the home of alternative rankings, medal solutions, and more";
    public override string MetadataThemeColor => "#353d55";
    public override string MetadataUrl => "https://osekai.net/home";

    public Home(
        CurrentSession currentSession, 
        CachedAuthenticatedOsuApiV2Interface cachedAuthenticatedOsuApiV2Interface, 
        CachedOsekaiDataAdapter osekaiDataAdapter, 
        IDatabaseUnitOfWorkFactory databaseUnitOfWorkFactory) 
        : base(currentSession, cachedAuthenticatedOsuApiV2Interface, osekaiDataAdapter, databaseUnitOfWorkFactory, -1)
    {
    }
}