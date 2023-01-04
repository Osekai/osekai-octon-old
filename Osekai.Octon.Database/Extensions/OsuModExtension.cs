using System.Collections.Immutable;
using System.Text;
using Osekai.Octon.Database.Enums;

namespace Osekai.Octon.Database.Extensions;

public static class OsuModExtension
{
    public static string? GetShortName(this OsuMod osuMod)
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

}