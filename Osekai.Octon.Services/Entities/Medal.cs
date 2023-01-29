using System.Diagnostics;
using Osekai.Octon.Enums;
using Osekai.Octon.Persistence;
using Osekai.Octon.Persistence.Dtos;
using Osekai.Octon.Persistence.Repositories;
using Osekai.Octon.Services.Extensions;

namespace Osekai.Octon.Services.Entities;

public class Medal
{
    protected internal IUnitOfWork UnitOfWork { get; }

    protected internal Medal(int id, string name, Uri link, string description, string? restriction, string grouping, string? instructions,
        int ordering, string? video, DateTimeOffset? date, DateTimeOffset? firstAchievedDate, string? firstAchievedBy, float rarity, int timesOwned,
        IUnitOfWork unitOfWork)
    {
        Id = id;
        Name = name;
        Link = link;
        Description = description;
        Restriction = restriction;
        Grouping = grouping;
        Instructions = instructions;
        Ordering = ordering;
        Video = video;
        Date = date;
        FirstAchievedDate = firstAchievedDate;
        FirstAchievedBy = firstAchievedBy;
        Rarity = rarity;
        TimesOwned = timesOwned;
        UnitOfWork = unitOfWork;
    }

    private string _name = null!;
    private Uri _uri = null!;
    private string _description = null!;
    private string? _restriction;
    private string _grouping = null!;
    private string? _instruction;
    private string? _video;
    private string? _firstAchievedBy;
    
    public int Id { get; }
    public string Name
    {
        get => _name;
        set
        {
            if (value.Length is < Specifications.MedalNameMinLength or > Specifications.MedalNameMaxLength)
                throw new ArgumentOutOfRangeException(nameof(value), value.Length, $"Invalid {nameof(Name)} length");
            
            _name = value;
        }
    }

    public Uri Link
    {
        get => _uri;
        
        set
        {
            // It doesn't allocate a new string
            int length = value.ToString().Length;
            
            if (length is < Specifications.MedalLinkMinLength or > Specifications.MedalLinkMaxLength)
                throw new ArgumentOutOfRangeException(nameof(value), length, $"Invalid {nameof(Link)} length");

            _uri = value;
        }
    }

    public string Description
    {
        get => _description;
        
        set
        {
            if (value.Length is < Specifications.MedalDescriptionMinLength or > Specifications.MedalDescriptionMaxLength)
                throw new ArgumentOutOfRangeException(nameof(value), value.Length, $"Invalid {nameof(Description)} length");

            _description = value;
        } 
    }

    public string? Restriction
    {
        get => _restriction;
        
        set
        {
            if (value == null)
            {
                _restriction = null;
                return;
            }

            if (value.Length is < Specifications.MedalRestrictionMinLength or > Specifications.MedalRestrictionMaxLength)
                throw new ArgumentOutOfRangeException(nameof(value), value.Length, $"Invalid {nameof(Restriction)} length");

            _restriction = value;
        }
    }

    public string Grouping
    {
        get => _grouping;
        set
        {
            if (value.Length is < Specifications.MedalGroupingMinLength or > Specifications.MedalGroupingMaxLength)
                throw new ArgumentOutOfRangeException(nameof(value), value.Length, $"Invalid {nameof(Grouping)} length");

            _grouping = value;
        }
    }

    public string? Instructions
    {
        get => _instruction;
        set
        {
            if (value == null)
            {
                _instruction = null;
                return;
            }
            
            if (value.Length is < Specifications.MedalInstructionsMinLength or > Specifications.MedalInstructionsMaxLength)
                throw new ArgumentOutOfRangeException(nameof(value), value.Length, $"Invalid {nameof(Grouping)} length");

            _instruction = value;
        }
    }

    public int Ordering { get; }
    public string? Video
    {
        get => _video;
        set
        {
            if (value == null)
            {
                _video = null;
                return;
            }
            
            if (value.Length is < Specifications.MedalVideoMinLength or > Specifications.MedalVideoMaxLength)
                throw new ArgumentOutOfRangeException(nameof(value), value.Length, $"Invalid {nameof(Grouping)} length");

            _video = value;
        }
    }
    public DateTimeOffset? Date { get; set; }
    public DateTimeOffset? FirstAchievedDate { get; set; }
    public string? FirstAchievedBy
    {
        get => _firstAchievedBy;
        set
        {
            if (value == null)
            {
                _firstAchievedBy = null;
                return;
            }
            
            if (value.Length is < Specifications.FirstAchievedByMinLength or > Specifications.FirstAchievedByMaxLength)
                throw new ArgumentOutOfRangeException(nameof(value), value.Length, $"Invalid {nameof(Grouping)} length");

            _firstAchievedBy = value;
        }
    }
    
    public float Rarity { get; set; }
    public int TimesOwned { get; set; }

    public async Task<MedalSettings?> GetSettingsAsync(CancellationToken cancellationToken = default)
    {
        MedalSettingsDto? medalSettingsDto = await UnitOfWork.MedalSettingsRepository.GetMedalSettingsByMedalIdAsync(Id, cancellationToken);
        return medalSettingsDto?.ToEntity();
    }
    
    public async Task<MedalSolution?> GetSolutionAsync(CancellationToken cancellationToken = default)
    {
        MedalSolutionDto? medalSettingsDto = await UnitOfWork.MedalSolutionRepository.GetMedalSolutionByMedalIdAsync(Id, cancellationToken);
        return medalSettingsDto?.ToEntity();
    }

    public async Task<IReadOnlyDictionary<OsuGamemode, BeatmapPack>> GetBeatmapPacks(CancellationToken cancellationToken = default)
    {
        IReadOnlyDictionary<OsuGamemode, BeatmapPackDto>? beatmapPacks = 
            await UnitOfWork.BeatmapPackRepository.GetBeatmapPacksByMedalId(Id, cancellationToken);
        
        Debug.Assert(beatmapPacks != null);

        return beatmapPacks!.ToDictionary(e => e.Key, e => e.Value.ToEntity(UnitOfWork));
    }
}