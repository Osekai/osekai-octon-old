using Osekai.Octon.Database.Dtos;
using Osekai.Octon.Database.Enums;

namespace Osekai.Octon.Database.EntityFramework.MySql.Models;

internal sealed class Medal
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public Uri Link { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? Restriction { get; set; } 
    public string Grouping { get; set; }= null!;
    public string? Instructions { get; set; } 
    public int Ordering { get; set; }
    public string? Video { get; set; }
    public DateTimeOffset? Date { get; set; }
    public DateTimeOffset? FirstAchievedDate { get; set; }

    public ICollection<BeatmapPackForMedal> BeatmapPacksForMedal { get; set; } = null!;
    public MedalSolution? Solution { get; set; }
    public MedalRarity? Rarity { get; set; }
    public MedalSettings? Settings { get; set; } 
    
    public string? FirstAchievedBy { get; set; }

    
    public MedalDto ToDto()
    {
        return new MedalDto(Id, Name, Link, Description, Grouping, Ordering,
            BeatmapPacksForMedal.Select(b => (b.BeatmapPack.ToDto(), b.Gamemode)), Solution?.ToDto(), Settings?.ToDto(),
            Restriction, Instructions, Video, Date, FirstAchievedDate, FirstAchievedBy, Rarity?.Frequency ?? 0, Rarity?.Count ?? 0);
    }
}
