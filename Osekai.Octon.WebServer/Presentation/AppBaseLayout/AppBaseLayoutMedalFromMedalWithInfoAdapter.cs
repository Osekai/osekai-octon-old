using System.Collections.Immutable;
using System.Text;
using Microsoft.Extensions.ObjectPool;
using Osekai.Octon.Enums;
using Osekai.Octon.Objects;
using Osekai.Octon.Objects.Aggregators;

namespace Osekai.Octon.WebServer.Presentation.AppBaseLayout;

public class AppBaseLayoutMedalFromMedalWithInfoAdapter: IAdapter<IReadOnlyMedalWithInfo, AppBaseLayoutMedal>
{   
    protected ObjectPool<StringBuilder> StringBuilderObjectPool { get; }

    public AppBaseLayoutMedalFromMedalWithInfoAdapter(ObjectPool<StringBuilder> stringBuilderObjectPool)
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

    private string GetPackIdString(IReadOnlyDictionary<OsuGamemode, IReadOnlyBeatmapPack> beatmapPacks)
    {
        if (beatmapPacks.Count == 0)
            return string.Empty;
        
        StringBuilder stringBuilder = StringBuilderObjectPool.Get();

        try
        {
            for (int i = 0; i < 4; i++)
            {
                if (beatmapPacks.TryGetValue((OsuGamemode)i, out IReadOnlyBeatmapPack? value))
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
    
    public Task<AppBaseLayoutMedal> AdaptAsync(IReadOnlyMedalWithInfo m, CancellationToken cancellationToken)
    {
        return Task.FromResult(new AppBaseLayoutMedal
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
            PackId = GetPackIdString(m.MedalBeatmapPacks),
        });
    }
}