using System.Collections.Immutable;
using System.Text;
using Microsoft.Extensions.ObjectPool;
using Osekai.Octon.Database;
using Osekai.Octon.Database.Dtos;
using Osekai.Octon.Database.Enums;

namespace Osekai.Octon.DataAdapter;

public class OsekaiDataAdapter
{   
    private readonly IDatabaseUnitOfWorkFactory _unitOfWorkFactory;
    private readonly ObjectPool<StringBuilder> _stringBuilderObjectPool;

    public OsekaiDataAdapter(ObjectPool<StringBuilder> stringBuilderObjectPool, IDatabaseUnitOfWorkFactory unitOfWorkFactory)
    {
        _stringBuilderObjectPool = stringBuilderObjectPool;
        _unitOfWorkFactory = unitOfWorkFactory;
    }

    private static readonly ImmutableArray<OsuMod> KnownMods = Enum.GetValues<OsuMod>().ToImmutableArray();

    private static string? GetModShortName(OsuMod osuMod)
    {
        return osuMod switch
        {
            OsuMod.Easy => "EZ",
            OsuMod.Perfect => "PF",
            OsuMod.Flashlight => "FL",
            OsuMod.SuddenDeath => "SD",
            OsuMod.Hidden => "HD",
            OsuMod.DoubleTime => "DT",
            OsuMod.HalfTime => "HT",
            OsuMod.Nightcore => "NC",
            OsuMod.NoFail => "NF",
            OsuMod.Relax => "RX",
            OsuMod.HardRock => "HR",
            _ => null
        };
    }

    private string GetModStringShortName(OsuMod mod)
    {
        if (mod == 0)
            return string.Empty;
        
        StringBuilder stringBuilder = _stringBuilderObjectPool.Get();

        try
        {
            for (int i = 1; i < KnownMods.Length; i++)
            {
                OsuMod knownMod = KnownMods[i];
                if ((mod & knownMod) != knownMod)
                    continue;
                    
                stringBuilder.Append(GetModShortName(knownMod));
                stringBuilder.Append(',');
            }

            stringBuilder.Remove(stringBuilder.Length - 1, 1);

            return stringBuilder.ToString();
        }
        finally
        {
            _stringBuilderObjectPool.Return(stringBuilder);
        }
    }

    private string GetPackIdString(IReadOnlyDictionary<OsuGamemode, BeatmapPackDto> beatmapPacks)
    {
        if (beatmapPacks.Count == 0)
            return string.Empty;
        
        StringBuilder stringBuilder = _stringBuilderObjectPool.Get();

        try
        {
            for (int i = 0; i < 4; i++)
            {
                if (beatmapPacks.TryGetValue((OsuGamemode)i, out BeatmapPackDto? value))
                    stringBuilder.Append(value.Id);

                if (i != 3)
                    stringBuilder.Append(',');
            }

            return stringBuilder.ToString();
        }
        finally
        {
            _stringBuilderObjectPool.Return(stringBuilder);
        }
    }
    
    public async Task<IAsyncEnumerable<OsekaiMedalData>> GetMedalDataAsync(CancellationToken cancellationToken)
    { 
        IDatabaseUnitOfWork unitOfWork = await _unitOfWorkFactory.CreateAsync(cancellationToken);
        IAsyncEnumerable<MedalDto> medals = await unitOfWork.MedalRepository.GetMedalsAsync(cancellationToken: cancellationToken);

        return medals.Select(m => new OsekaiMedalData
        {
            Date = m.Date?.UtcDateTime.ToShortDateString(),
            Description = m.Description,
            Grouping = m.Grouping,
            Instructions = m.Instructions,
            Link = m.Link,
            Locked = (m.Settings?.Locked ?? false) ? 1 : 0,
            Mods = GetModStringShortName(m.Solution?.Mods ?? 0),
            ModeOrder = m.Restriction switch
            {
                "osu" => 2,
                "taiko" => 3,
                "fruits" => 4,
                "mania" => 5,
                _ => 1
            },
            Rarity = m.Rarity,
            FirstAchievedBy = m.FirstAchievedBy,
            FirstAchievedDate = m.FirstAchievedDate?.UtcDateTime.ToShortDateString(),
            Name = m.Name,
            Ordering = m.Ordering,
            Restriction = m.Restriction,
            Solution = m.Solution?.Text,
            Video = m.Video,
            MedalId = m.Id,
            PackId = GetPackIdString(m.BeatmapPacks),
        });
    }
}