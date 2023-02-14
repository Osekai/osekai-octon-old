using Osekai.Octon.Models;

namespace Osekai.Octon.Persistence.EntityFramework.MySql.Dtos;

internal sealed class MedalDto: IReadOnlyMedal
{
    public MedalDto(int id, string name, Uri link, string description, string grouping, int ordering,
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
        FirstAchievedDate = firstAchievedDate;
        FirstAchievedBy = firstAchievedBy;
        Rarity = rarity;
        TimesOwned = timesOwned;
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
    public float Rarity { get; }
    public int TimesOwned { get; }
}