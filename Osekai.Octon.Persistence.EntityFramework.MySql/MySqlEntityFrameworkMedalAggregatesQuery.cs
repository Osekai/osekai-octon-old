using Microsoft.EntityFrameworkCore;
using Osekai.Octon.Models;
using Osekai.Octon.Persistence.EntityFramework.MySql.Dtos;
using Osekai.Octon.Persistence.EntityFramework.MySql.Entities;
using Osekai.Octon.Query;
using Osekai.Octon.Query.QueryResults;

namespace Osekai.Octon.Persistence.EntityFramework.MySql;

public class MySqlEntityFrameworkMedalAggregatesQuery: IQuery<IEnumerable<IReadOnlyMedalAggregateQueryResult>>
{
    protected MySqlOsekaiDbContext Context { get; }

    public MySqlEntityFrameworkMedalAggregatesQuery(MySqlOsekaiDbContext context)
    {
        Context = context;
    }

    public async Task<IEnumerable<IReadOnlyMedalAggregateQueryResult>> ExecuteAsync(CancellationToken cancellationToken)
    {
        IEnumerable<Medal> medals = await Context.Medals.Include(e => e.BeatmapPacksForMedal)
            .ThenInclude(e => e.BeatmapPack)
            .Include(e => e.Solution)
            .Include(e => e.Settings)
            .ToArrayAsync(cancellationToken);

        return medals.Select(b => new MedalAggregateQueryResultDto {
            Medal = b.ToDto(),
            MedalBeatmapPacks = b.BeatmapPacksForMedal.ToDictionary(e => e.Gamemode, e => (IReadOnlyBeatmapPack) e.BeatmapPack.ToDto()),
            MedalSettings = b.Settings?.ToDto(),
            MedalSolution = b.Solution?.ToDto()
        });
    }
}