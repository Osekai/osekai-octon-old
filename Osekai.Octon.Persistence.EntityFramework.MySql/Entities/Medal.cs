﻿namespace Osekai.Octon.Persistence.EntityFramework.MySql.Entities;

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

    
    public Domain.AggregateRoots.Medal ToAggregateRoot()
    {
        return new Domain.AggregateRoots.Medal(Id, Name, Link, Description, Restriction, Grouping, Instructions, Ordering, Video, Date, Rarity?.Frequency ?? 0, Rarity?.Count ?? 0);
    }
}
