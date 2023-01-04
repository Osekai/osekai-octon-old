using Osekai.Octon.Database.Enums;

namespace Osekai.Octon.Database.Dtos;

public class MedalSolutionDto
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