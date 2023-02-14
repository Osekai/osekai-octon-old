using Osekai.Octon.Enums;
using Osekai.Octon.Models;

namespace Osekai.Octon.Persistence.Repositories;

public interface IBeatmapPackRepository
{
    Task<IReadOnlyDictionary<OsuGamemode, IReadOnlyBeatmapPack>?> GetBeatmapPacksByMedalId(int medalId, CancellationToken cancellationToken = default);
}