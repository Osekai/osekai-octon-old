using Osekai.Octon.Enums;
using Osekai.Octon.Persistence.Dtos;

namespace Osekai.Octon.Persistence.Repositories;

public interface IBeatmapPackRepository
{
    Task<IReadOnlyDictionary<OsuGamemode, BeatmapPackDto>?> GetBeatmapPacksByMedalId(int medalId, CancellationToken cancellationToken = default);
}