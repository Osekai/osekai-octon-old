using Osekai.Octon.Enums;

namespace Osekai.Octon.RichModels;

public class MedalSolution
{
    protected internal MedalSolution(string text, string submittedBy, OsuMod mods)
    {
        Text = text;
        SubmittedBy = submittedBy;
        Mods = mods;
    }
    
    public string Text { get; }
    public string SubmittedBy { get; }
    public OsuMod Mods { get; }
}