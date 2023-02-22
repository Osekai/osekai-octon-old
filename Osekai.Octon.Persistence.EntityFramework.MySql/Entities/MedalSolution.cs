using Osekai.Octon.Domain.Enums;

namespace Osekai.Octon.Persistence.EntityFramework.MySql.Entities;

internal sealed class MedalSolution
{
    public int Id { get; set;  }
    public int MedalId { get; set; }
    
    public Medal Medal { get; set; } = null!;
    
    public string Text { get; set; } = null!;
    public string SubmittedBy { get; set; } = null!;
    public OsuMod Mods { get; set; }

    public Domain.ValueObjects.MedalSolution ToValueObject()
    {
        return new Domain.ValueObjects.MedalSolution(Text, SubmittedBy, Mods);
    }
}