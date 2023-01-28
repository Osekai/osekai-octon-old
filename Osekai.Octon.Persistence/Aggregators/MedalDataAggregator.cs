using Osekai.Octon.Enums;
using Osekai.Octon.Persistence.Dtos;

namespace Osekai.Octon.Persistence.Aggregators;


public interface IMedalDataAggregator: IDataAggregator<IEnumerable<IMedalDataAggregator.AggregatedMedalData>>
{
    public readonly struct AggregatedMedalData
    {
        public MedalDto Medal { get; init; }
        public MedalSettingsDto? MedalSettings { get; init; }
        public MedalSolutionDto? MedalSolution { get; init; }
        public IReadOnlyDictionary<OsuGamemode, BeatmapPackDto> BeatmapPacks { get; init; }
    }
}