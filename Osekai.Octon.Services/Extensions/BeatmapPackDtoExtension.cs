using Osekai.Octon.Persistence;
using Osekai.Octon.Persistence.Dtos;
using Osekai.Octon.Services.Entities;

namespace Osekai.Octon.Services.Extensions;

internal static class BeatmapPackDtoExtension
{
    internal static BeatmapPack ToEntity(this BeatmapPackDto beatmapPackDto, IDatabaseUnitOfWork unitOfWork)
    {
        return new BeatmapPack(beatmapPackDto.Id, beatmapPackDto.BeatmapCount, unitOfWork);
    }
}