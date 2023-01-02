namespace Osekai.Octon.Database.Dtos;

public class MedalDto
{
    public MedalDto(int id, string name, Uri link, string description, string grouping, int ordering,
        IEnumerable<BeatmapPackDto> beatmapPacks, 
        string? restriction = null, string? instructions = null, string? video = null, DateTimeOffset? date = null,
        DateTimeOffset? firstAchievedDate = null, string? firstAchievedBy = null)
    {
        Id = id;
        Name = name;
        Link = link;
        Description = description;
        Grouping = grouping;
        Ordering = ordering;
        Restriction = restriction;
        Video = video;
        Date = date;
        Instructions = instructions;
        BeatmapPacks = new List<BeatmapPackDto>(beatmapPacks);
        FirstAchievedDate = firstAchievedDate;
        FirstAchievedBy = firstAchievedBy;
    }

    public int Id { get; }
    public string Name { get; }
    public Uri Link { get; }
    public string Description { get; }
    public string? Restriction { get; } 
    public string Grouping { get; } 
    public string? Instructions { get; } 
    public int Ordering { get; }
    public string? Video { get; }
    public DateTimeOffset? Date { get; }
    public DateTimeOffset? FirstAchievedDate { get; }
    public string? FirstAchievedBy { get; }
    public IReadOnlyCollection<BeatmapPackDto> BeatmapPacks { get; }
}