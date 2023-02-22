using Osekai.Octon.Domain.Enums;

namespace Osekai.Octon.Domain.ValueObjects;

public readonly struct MedalSolution
{
    public MedalSolution(string text, string submittedBy, OsuMod mods)
    {
        Text = text;
        SubmittedBy = submittedBy;
        Mods = mods;
    }

    private readonly string _text = null!;
    private readonly string _submittedBy = null!;

    public string Text
    {
        get => _text;
        init
        {
            //if (value.Length is < Specifications.MedalSolutionTextMinLength or > Specifications.MedalSolutionTextMaxLength)
                //throw new ArgumentOutOfRangeException(nameof(value), value.Length, $"Invalid {nameof(Text)} length");

            _text = value;
        }
    }

    public string SubmittedBy
    {
        get => _submittedBy;
        init
        {
            if (value.Length is < Specifications.MedalSolutionSubmittedByMinLength or > Specifications.MedalSolutionSubmittedByMaxLength)
                throw new ArgumentOutOfRangeException(nameof(value), value.Length, $"Invalid {nameof(SubmittedBy)} length");

            _submittedBy = value;
        }
    }

    public OsuMod Mods { get; }
}