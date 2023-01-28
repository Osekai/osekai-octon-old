using Microsoft.EntityFrameworkCore;
using Osekai.Octon.Persistence.Aggregators;
using Osekai.Octon.Persistence.EntityFramework.MySql.Models;

namespace Osekai.Octon.Persistence.EntityFramework.MySql;

public class MySqlEntityFrameworkMedalDataAggregator: IMedalDataAggregator
{
    protected MySqlOsekaiDbContext Context { get; }

    public MySqlEntityFrameworkMedalDataAggregator(MySqlOsekaiDbContext context)
    {
        Context = context;
    }
    
    public async Task<IEnumerable<IMedalDataAggregator.AggregatedMedalData>> AggregateAsync(CancellationToken cancellationToken)
    {
        IEnumerable<Medal> medals = await Context.Medals.Include(e => e.BeatmapPacksForMedal)
            .ThenInclude(e => e.BeatmapPack)
            .Include(e => e.Solution)
            .Include(e => e.Settings)
            .ToArrayAsync(cancellationToken);

        return medals.Select(b => new IMedalDataAggregator.AggregatedMedalData
        {
            Medal = b.ToDto(),
            BeatmapPacks = b.BeatmapPacksForMedal.ToDictionary(e => e.Gamemode, e => e.BeatmapPack.ToDto()),
            MedalSettings = b.Settings?.ToDto(),
            MedalSolution = b.Solution?.ToDto()
        });
    }
}