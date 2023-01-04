using Osekai.Octon.Database.Enums;

namespace Osekai.Octon.Database.Dtos;

public class MedalDto
{
    public MedalDto(int id, string name, Uri link, string description, string grouping, int ordering,
        IEnumerable<(BeatmapPackDto BeatmapPack, OsuGamemode Gamemode)> beatmapPacks, 
        MedalSolutionDto? solution = null, MedalSettingsDto? settings = null,
        string? restriction = null, string? instructions = null, string? video = null, DateTimeOffset? date = null,
        DateTimeOffset? firstAchievedDate = null, string? firstAchievedBy = null, float rarity = 0, int timesOwned = 0)
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
        BeatmapPacks = beatmapPacks.ToDictionary(t => t.Gamemode, t => t.BeatmapPack);
        FirstAchievedDate = firstAchievedDate;
        FirstAchievedBy = firstAchievedBy;
        Solution = solution;
        Rarity = rarity;
        TimesOwned = timesOwned;
        Settings = settings;
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
    public IReadOnlyDictionary<OsuGamemode, BeatmapPackDto> BeatmapPacks { get; }
    public MedalSolutionDto? Solution { get; }
    public MedalSettingsDto? Settings { get; }
    public float Rarity { get; }
    public int TimesOwned { get; }
}