using Osekai.Octon.Database.Enums;
using Osekai.Octon.Database.Models;

namespace Osekai.Octon.Database.EntityFramework.MySql;

public class MySqlTestDataPopulator: ITestDataPopulator
{
    private readonly MySqlOsekaiDbContext _context;
    
    public MySqlTestDataPopulator(MySqlOsekaiDbContext context)
    {
        _context = context;
    }
    
    public async Task PopulateDatabaseAsync(CancellationToken cancellationToken = default)
    {
        _context.Apps.Add(
            new App
            {
                Name = "Home",
                Id = -1,
                Order = 1,
                Experimental = false,
                Visible = true,
                SimpleName = "home",
                AppTheme = new AppTheme
                {
                    Color = "53,61,85",
                    DarkColor = "53,61,85",
                    HasCover = true,
                    HslValueMultiplier = 1,
                    DarkHslValueMultiplier = 1
                }
            });


        Medal medal = new Medal
        {
            Date = new DateTimeOffset(2008, 08, 26, 0, 0, 0, TimeSpan.Zero),
            Description = "I-it's not like I'm proud of you or anything..",
            Grouping = "Beatmap Packs",
            Link = new Uri("https://assets.ppy.sh/medals/web/all-packs-anime-1.png"),
            BeatmapPacksForMedal = new HashSet<BeatmapPackForMedal>(),
            Name = "Anime Pack vol.1",
            Ordering = 1
        };

        BeatmapPack beatmapPack = new BeatmapPack { Id = 43, BeatmapCount = 14 };
        
        _context.BeatmapPacks.Add(beatmapPack);
        _context.Medals.Add(medal);
        _context.BeatmapPacksForMedals.Add(new BeatmapPackForMedal { Medal = medal, BeatmapPack = beatmapPack });

        await _context.SaveChangesAsync(cancellationToken);
    }
}