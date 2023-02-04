using Osekai.Octon.Enums;

namespace Osekai.Octon.Objects.Aggregators;

public interface IReadOnlyMedalWithInfo
{
    public IReadOnlyMedal Medal { get; }
    public IReadOnlyMedalSettings? MedalSettings { get ;}
    public IReadOnlyMedalSolution? MedalSolution { get; }
    public IReadOnlyDictionary<OsuGamemode, IReadOnlyBeatmapPack> MedalBeatmapPacks { get; }
}