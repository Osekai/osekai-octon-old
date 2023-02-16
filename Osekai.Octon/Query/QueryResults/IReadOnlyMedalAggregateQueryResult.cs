using Osekai.Octon.Enums;
using Osekai.Octon.Models;

namespace Osekai.Octon.Query.QueryResults;

public interface IReadOnlyMedalAggregateQueryResult
{
    public IReadOnlyMedal Medal { get; }
    public IReadOnlyMedalSettings? MedalSettings { get ;}
    public IReadOnlyMedalSolution? MedalSolution { get; }
    public IReadOnlyDictionary<OsuGamemode, IReadOnlyBeatmapPack> MedalBeatmapPacks { get; }
}