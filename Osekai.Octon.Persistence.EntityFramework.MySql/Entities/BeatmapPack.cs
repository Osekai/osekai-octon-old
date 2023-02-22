namespace Osekai.Octon.Persistence.EntityFramework.MySql.Entities;

internal sealed class BeatmapPack
{
    public int Id { get; set; }
    public int BeatmapCount { get; set; }

    public ICollection<BeatmapPackForMedal> MedalsForBeatmapPack { get; set; } = null!;

    public Domain.Aggregates.BeatmapPack ToAggregate()
    {
        return new Domain.Aggregates.BeatmapPack(Id, BeatmapCount);
    }
}