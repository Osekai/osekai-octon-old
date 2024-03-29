﻿using Osekai.Octon.Domain.AggregateRoots;
using Osekai.Octon.Domain.Enums;

namespace Osekai.Octon.Domain.Repositories;

public interface IBeatmapPackRepository
{
    Task<IReadOnlyDictionary<OsuGamemode, BeatmapPack>?> GetBeatmapPacksByMedalId(int medalId, CancellationToken cancellationToken = default);
}