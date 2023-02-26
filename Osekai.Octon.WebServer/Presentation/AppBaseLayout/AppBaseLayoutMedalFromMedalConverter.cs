using System.Collections.Immutable;
using System.Text;
using Microsoft.Extensions.ObjectPool;
using Osekai.Octon.Domain.AggregateRoots;
using Osekai.Octon.Domain.Enums;

namespace Osekai.Octon.WebServer.Presentation.AppBaseLayout;

public class AppBaseLayoutMedalFromMedalConverter: IConverter<Medal, AppBaseLayoutMedal>
{   
    protected ObjectPool<StringBuilder> StringBuilderObjectPool { get; }

    public AppBaseLayoutMedalFromMedalConverter(ObjectPool<StringBuilder> stringBuilderObjectPool)
    {
        StringBuilderObjectPool = stringBuilderObjectPool;
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

    private string GetPackIdString(IReadOnlyDictionary<OsuGamemode, BeatmapPack> beatmapPacks)
    {
        if (beatmapPacks.Count == 0)
            return string.Empty;
        
        StringBuilder stringBuilder = StringBuilderObjectPool.Get();

        try
        {
            for (int i = 0; i < 4; i++)
            {
                if (beatmapPacks.TryGetValue((OsuGamemode)i, out BeatmapPack? value))
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
    
    public ValueTask<AppBaseLayoutMedal> ConvertAsync(Medal m, CancellationToken cancellationToken)
    {
        return ValueTask.FromResult(new AppBaseLayoutMedal
        {
            Date = m.Date?.UtcDateTime.ToShortDateString(),
            Description = m.Description,
            Grouping = m.Grouping,
            Instructions = m.Instructions,
            Link = m.Link,
            Locked = (m.MedalSettings!.Value.Value?.Locked ?? false) ? 1 : 0,
            Mods = GetModStringShortName(m.MedalSolution!.Value.Value?.Mods ?? OsuMod.None),
            ModeOrder = m.Restriction switch
            {
                "osu" => 2,
                "taiko" => 3,
                "fruits" => 4,
                "mania" => 5,
                _ => 1
            },
            Rarity = m.Rarity,
            FirstAchievedBy = m.FirstAchievement!.Value.Value?.FirstAchievedBy,
            FirstAchievedDate = m.FirstAchievement!.Value.Value?.FirstAchievedDate.UtcDateTime.ToShortDateString(),
            Name = m.Name,
            Ordering = m.Ordering,
            Restriction = m.Restriction,
            Solution = m.MedalSolution.Value.Value?.Text,
            Video = m.Video,
            MedalId = m.Id,
            PackId = GetPackIdString(m.BeatmapPacks!.Value.Value),
        });
    }
}