using Osekai.Octon.Database.Enums;

namespace Osekai.Octon.Database.Models;

public class BeatmapPackForMedal
{
    public Medal Medal { get; set; } = null!;
    public BeatmapPack BeatmapPack { get; set; } = null!;
    
    public int MedalId { get; set; }
    public int BeatmapPackId { get; set; }
    public OsuGamemode Gamemode { get; set; }
}