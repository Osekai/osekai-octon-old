using Osekai.Octon.Models;
using Osekai.Octon.Persistence;

namespace Osekai.Octon.RichModels.Extensions;

internal static class ReadOnlyBeatmapPackExtension
{
    public static BeatmapPack ToRichModel(this IReadOnlyBeatmapPack beatmapPackDto, IUnitOfWork unitOfWork)
    {
        return new BeatmapPack(beatmapPackDto.Id, beatmapPackDto.BeatmapCount, unitOfWork);
    }
}