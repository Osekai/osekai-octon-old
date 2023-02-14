using Microsoft.EntityFrameworkCore;
using Osekai.Octon.Enums;
using Osekai.Octon.Models;
using Osekai.Octon.Persistence.EntityFramework.MySql.Dtos;
using Osekai.Octon.Persistence.EntityFramework.MySql.Entities;
using Osekai.Octon.Persistence.Repositories;

namespace Osekai.Octon.Persistence.EntityFramework.MySql.Repositories;

public class MySqlEntityFrameworkBeatmapPackRepository: IBeatmapPackRepository
{
    protected MySqlOsekaiDbContext Context { get; }
    
    public MySqlEntityFrameworkBeatmapPackRepository(MySqlOsekaiDbContext context)
    {
        Context = context;
    }
    
    public async Task<IReadOnlyDictionary<OsuGamemode, IReadOnlyBeatmapPack>?> GetBeatmapPacksByMedalId(int medalId, CancellationToken cancellationToken = default)
    {
        Medal? medal = await Context.Medals
            .Include(b => b.BeatmapPacksForMedal).ThenInclude(b => b.BeatmapPack)
            .Where(m => m.Id == medalId)
            .FirstOrDefaultAsync();

        if (medal == null)
            return null;

        return medal.BeatmapPacksForMedal.ToDictionary(e => e.Gamemode, e => (IReadOnlyBeatmapPack)e.BeatmapPack.ToDto());
    }
}