using Osekai.Octon.Enums;

namespace Osekai.Octon.Persistence.EntityFramework.MySql.Entities;

internal sealed class BeatmapPackForMedal
{
    public Medal Medal { get; set; } = null!;
    public BeatmapPack BeatmapPack { get; set; } = null!;
    
    public int MedalId { get; set; }
    public int BeatmapPackId { get; set; }
    public OsuGamemode Gamemode { get; set; }
}