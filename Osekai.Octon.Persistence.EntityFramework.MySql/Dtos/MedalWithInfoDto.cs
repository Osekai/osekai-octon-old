using Osekai.Octon.Enums;
using Osekai.Octon.Models;
using Osekai.Octon.Persistence.QueryResults;

namespace Osekai.Octon.Persistence.EntityFramework.MySql.Dtos;

internal class MedalAggregateQueryResultDto: IReadOnlyMedalAggregateQueryResult
{
    public IReadOnlyMedal Medal { get; init; }= null!;
    public IReadOnlyMedalSettings? MedalSettings { get; init; } = null;
    public IReadOnlyMedalSolution? MedalSolution { get; init;} = null;
    public IReadOnlyDictionary<OsuGamemode, IReadOnlyBeatmapPack> MedalBeatmapPacks { get; init; } = null!;
}