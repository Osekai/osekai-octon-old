namespace Osekai.Octon.Domain.Aggregates;

public class BeatmapPack
{
    public BeatmapPack(int id, int beatmapCount)
    {
        Id = id;
        BeatmapCount = beatmapCount;
    }
    
    public int Id { get; }
    public int BeatmapCount { get; set; }
}