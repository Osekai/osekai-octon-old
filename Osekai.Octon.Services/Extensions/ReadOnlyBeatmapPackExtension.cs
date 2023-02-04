using Osekai.Octon.Objects;
using Osekai.Octon.Persistence;
using Osekai.Octon.Services.Entities;

namespace Osekai.Octon.Services.Extensions;

internal static class ReadOnlyBeatmapPackExtension
{
    internal static BeatmapPack ToEntity(this IReadOnlyBeatmapPack beatmapPackDto, IUnitOfWork unitOfWork)
    {
        return new BeatmapPack(beatmapPackDto.Id, beatmapPackDto.BeatmapCount, unitOfWork);
    }
}