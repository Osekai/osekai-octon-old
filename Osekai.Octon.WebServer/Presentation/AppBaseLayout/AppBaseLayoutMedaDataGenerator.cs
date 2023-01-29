using System.Collections.Immutable;
using System.Text;
using Microsoft.Extensions.ObjectPool;
using Osekai.Octon.Enums;
using Osekai.Octon.Persistence.Aggregators;
using Osekai.Octon.Persistence.Dtos;

namespace Osekai.Octon.WebServer.Presentation.AppBaseLayout;

public class AppBaseLayoutMedaDataGenerator: IAppBaseLayoutMedalDataGenerator
{   
    protected ObjectPool<StringBuilder> StringBuilderObjectPool { get; }
    protected IMedalDataAggregator MedalDataAggregator { get; }

    public AppBaseLayoutMedaDataGenerator(ObjectPool<StringBuilder> stringBuilderObjectPool, IMedalDataAggregator medalDataAggregator)
    {
        StringBuilderObjectPool = stringBuilderObjectPool;
        MedalDataAggregator = medalDataAggregator;
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
            OsuMod.SpunOut => "SO",
            _ => null
        };
    }

    private string GetModStringShortName(OsuMod mod)
    {
        if (mod == 0)
            return string.Empty;
        
        StringBuilder stringBuilder = StringBuilderObjectPool.Get();

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
            StringBuilderObjectPool.Return(stringBuilder);
        }
    }

    private string GetPackIdString(IReadOnlyDictionary<OsuGamemode, BeatmapPackDto> beatmapPacks)
    {
        if (beatmapPacks.Count == 0)
            return string.Empty;
        
        StringBuilder stringBuilder = StringBuilderObjectPool.Get();

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
            StringBuilderObjectPool.Return(stringBuilder);
        }
    }
    
    public async Task<IEnumerable<AppBaseLayoutMedalData>> GenerateAsync(CancellationToken cancellationToken)
    {
        IEnumerable<IMedalDataAggregator.AggregatedMedalData> data = await MedalDataAggregator.AggregateAsync(cancellationToken);

        return data
            .Select(m =>
                new AppBaseLayoutMedalData
                {
                    Date = m.Medal.Date?.UtcDateTime.ToShortDateString(),
                    Description = m.Medal.Description,
                    Grouping = m.Medal.Grouping,
                    Instructions = m.Medal.Instructions,
                    Link = m.Medal.Link,
                    Locked = (m.MedalSettings?.Locked ?? false) ? 1 : 0,
                    Mods = GetModStringShortName(m.MedalSolution?.Mods ?? 0),
                    ModeOrder = m.Medal.Restriction switch
                    {
                        "osu" => 2,
                        "taiko" => 3,
                        "fruits" => 4,
                        "mania" => 5,
                        _ => 1
                    },
                    Rarity = m.Medal.Rarity,
                    FirstAchievedBy = m.Medal.FirstAchievedBy,
                    FirstAchievedDate = m.Medal.FirstAchievedDate?.UtcDateTime.ToShortDateString(),
                    Name = m.Medal.Name,
                    Ordering = m.Medal.Ordering,
                    Restriction = m.Medal.Restriction,
                    Solution = m.MedalSolution?.Text,
                    Video = m.Medal.Video,
                    MedalId = m.Medal.Id,
                    PackId = GetPackIdString(m.BeatmapPacks),
                });
    }
}