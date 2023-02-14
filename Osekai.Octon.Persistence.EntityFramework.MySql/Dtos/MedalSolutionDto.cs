using Osekai.Octon.Enums;
using Osekai.Octon.Models;

namespace Osekai.Octon.Persistence.EntityFramework.MySql.Dtos;

internal sealed class MedalSolutionDto: IReadOnlyMedalSolution
{
    public MedalSolutionDto(string text, string submittedBy, OsuMod mods)
    {
        Text = text;
        SubmittedBy = submittedBy;
        Mods = mods;
    }
    
    public string Text { get; }
    public string SubmittedBy { get; }
    public OsuMod Mods { get; }
}