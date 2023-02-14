namespace Osekai.Octon.Models;

public interface IReadOnlyMedal
{
    int Id { get; }
    string Name { get; }
    Uri Link { get; }
    string Description { get; }
    string? Restriction { get; } 
    string Grouping { get; } 
    string? Instructions { get; } 
    int Ordering { get; }
    string? Video { get; }
    DateTimeOffset? Date { get; }
    DateTimeOffset? FirstAchievedDate { get; }
    string? FirstAchievedBy { get; }
    float Rarity { get; }
    int TimesOwned { get; }
}