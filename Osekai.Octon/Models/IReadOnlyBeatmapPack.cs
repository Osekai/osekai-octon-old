namespace Osekai.Octon.Models;

public interface IReadOnlyBeatmapPack
{
    int Id { get; }
    int BeatmapCount { get; }
}