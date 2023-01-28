using Osekai.Octon.Persistence.Dtos;

namespace Osekai.Octon.Persistence.EntityFramework.MySql.Models;

internal sealed class BeatmapPack
{
    public int Id { get; set; }
    public int BeatmapCount { get; set; }

    public ICollection<BeatmapPackForMedal> MedalsForBeatmapPack { get; set; } = null!;

    public BeatmapPackDto ToDto()
    {
        return new BeatmapPackDto(Id, BeatmapCount);
    }
}