using Osekai.Octon.Enums;
using Osekai.Octon.Objects;
using Osekai.Octon.Objects.Aggregators;

namespace Osekai.Octon.Persistence.EntityFramework.MySql.Dtos;

internal class MedalWithInfoDto: IReadOnlyMedalWithInfo
{
    public IReadOnlyMedal Medal { get; init; }= null!;
    public IReadOnlyMedalSettings? MedalSettings { get; init; } = null;
    public IReadOnlyMedalSolution? MedalSolution { get; init;} = null;
    public IReadOnlyDictionary<OsuGamemode, IReadOnlyBeatmapPack> MedalBeatmapPacks { get; init; } = null!;
}