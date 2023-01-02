namespace Osekai.Octon.Database.Dtos;

public class BeatmapPackDto
{
    public BeatmapPackDto(int id, int beatmapCount)
    {
        Id = id;
        BeatmapCount = beatmapCount;
    }
    
    public int Id { get; }
    public int BeatmapCount { get; }
}