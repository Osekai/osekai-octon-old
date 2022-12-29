namespace Osekai.Octon.Database.Models;

public class BeatmapPack
{
    public int Id { get; set; }
    public int BeatmapCount { get; set; }

    public ICollection<BeatmapPackForMedal> MedalsForBeatmapPack { get; set; } = null!;
}