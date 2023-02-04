namespace Osekai.Octon.Objects;

public interface IReadOnlyBeatmapPack
{
    int Id { get; }
    int BeatmapCount { get; }
}