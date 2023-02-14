using Osekai.Octon.Models;

namespace Osekai.Octon.Persistence.EntityFramework.MySql.Dtos;

internal sealed class BeatmapPackDto: IReadOnlyBeatmapPack
{
    public BeatmapPackDto(int id, int beatmapCount)
    {
        Id = id;
        BeatmapCount = beatmapCount;
    }
    
    public int Id { get; }
    public int BeatmapCount { get; }
}