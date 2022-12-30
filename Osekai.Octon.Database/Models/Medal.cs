using System.Text.Json.Serialization;

namespace Osekai.Octon.Database.Models;

public class Medal
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
    
    public string? FirstAchievedBy { get; set; } 
}
