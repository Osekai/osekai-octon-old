using Microsoft.EntityFrameworkCore;
using Osekai.Octon.Enums;
using Osekai.Octon.Persistence.Dtos;
using Osekai.Octon.Persistence.EntityFramework.MySql.Models;
using Osekai.Octon.Persistence.Repositories;

namespace Osekai.Octon.Persistence.EntityFramework.MySql.Repositories;

public class MySqlEntityFrameworkBeatmapPackRepository: IBeatmapPackRepository
{
    protected MySqlOsekaiDbContext Context { get; }
    
    public MySqlEntityFrameworkBeatmapPackRepository(MySqlOsekaiDbContext context)
    {
        Context = context;
    }
    
    public async Task<IReadOnlyDictionary<OsuGamemode, BeatmapPackDto>?> GetBeatmapPacksByMedalId(int medalId, CancellationToken cancellationToken = default)
    {
        Medal? medal = await Context.Medals
            .Include(b => b.BeatmapPacksForMedal).ThenInclude(b => b.BeatmapPack)
            .Where(m => m.Id == medalId)
            .FirstOrDefaultAsync();

        if (medal == null)
            return null;

        return medal.BeatmapPacksForMedal.ToDictionary(e => e.Gamemode, e => e.BeatmapPack.ToDto());
    }
}