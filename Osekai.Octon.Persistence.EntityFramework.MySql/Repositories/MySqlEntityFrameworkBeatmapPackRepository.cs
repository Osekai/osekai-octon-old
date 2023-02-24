using Microsoft.EntityFrameworkCore;
using Osekai.Octon.Domain.Enums;
using Osekai.Octon.Domain.Repositories;
using Osekai.Octon.Persistence.EntityFramework.MySql.Entities;
using BeatmapPack = Osekai.Octon.Domain.AggregateRoots.BeatmapPack;

namespace Osekai.Octon.Persistence.EntityFramework.MySql.Repositories;

public class MySqlEntityFrameworkBeatmapPackRepository: IBeatmapPackRepository
{
    protected MySqlOsekaiDbContext Context { get; }
    
    public MySqlEntityFrameworkBeatmapPackRepository(MySqlOsekaiDbContext context)
    {
        Context = context;
    }
    
    public async Task<IReadOnlyDictionary<OsuGamemode, BeatmapPack>?> GetBeatmapPacksByMedalId(int medalId, CancellationToken cancellationToken = default)
    {
        Medal? medal = await Context.Medals
            .Include(b => b.BeatmapPacksForMedal).ThenInclude(b => b.BeatmapPack)
            .Where(m => m.Id == medalId)
            .FirstOrDefaultAsync();

        if (medal == null)
            return null;

        return medal.BeatmapPacksForMedal.ToDictionary(e => e.Gamemode, e => (BeatmapPack)e.BeatmapPack.ToAggregateRoot());
    }
}