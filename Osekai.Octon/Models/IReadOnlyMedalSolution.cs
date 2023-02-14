using Osekai.Octon.Enums;

namespace Osekai.Octon.Models;

public interface IReadOnlyMedalSolution
{
    string Text { get; }
    string SubmittedBy { get; }
    OsuMod Mods { get; }
}