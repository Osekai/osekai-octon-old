using Osekai.Octon.Database.Dtos;

namespace Osekai.Octon.Database.EntityFramework.MySql.Models;

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