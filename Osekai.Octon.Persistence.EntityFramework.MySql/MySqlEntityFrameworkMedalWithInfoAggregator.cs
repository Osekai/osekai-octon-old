using Microsoft.EntityFrameworkCore;
using Osekai.Octon.Objects;
using Osekai.Octon.Objects.Aggregators;
using Osekai.Octon.Persistence.EntityFramework.MySql.Dtos;
using Osekai.Octon.Persistence.EntityFramework.MySql.Models;

namespace Osekai.Octon.Persistence.EntityFramework.MySql;

public class MySqlEntityFrameworkMedalWithInfoAggregator: IAggregator<IReadOnlyMedalWithInfo>
{
    protected MySqlOsekaiDbContext Context { get; }

    public MySqlEntityFrameworkMedalWithInfoAggregator(MySqlOsekaiDbContext context)
    {
        Context = context;
    }

    public async Task<IEnumerable<IReadOnlyMedalWithInfo>> AggregateAllAsync(CancellationToken cancellationToken)
    {
        IEnumerable<Medal> medals = await Context.Medals.Include(e => e.BeatmapPacksForMedal)
            .ThenInclude(e => e.BeatmapPack)
            .Include(e => e.Solution)
            .Include(e => e.Settings)
            .ToArrayAsync(cancellationToken);

        return medals.Select(b => new MedalWithInfoDto {
            Medal = b.ToDto(),
            MedalBeatmapPacks = b.BeatmapPacksForMedal.ToDictionary(e => e.Gamemode, e => (IReadOnlyBeatmapPack) e.BeatmapPack.ToDto()),
            MedalSettings = b.Settings?.ToDto(),
            MedalSolution = b.Solution?.ToDto()
        });
    }
}