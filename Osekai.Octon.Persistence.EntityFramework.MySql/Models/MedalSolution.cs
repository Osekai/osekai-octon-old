using Osekai.Octon.Enums;
using Osekai.Octon.Persistence.EntityFramework.MySql.Dtos;

namespace Osekai.Octon.Persistence.EntityFramework.MySql.Models;

internal sealed class MedalSolution
{
    public int Id { get; set;  }
    public int MedalId { get; set; }
    
    public Medal Medal { get; set; } = null!;
    
    public string Text { get; set; } = null!;
    public string SubmittedBy { get; set; } = null!;
    public OsuMod Mods { get; set; }

    public MedalSolutionDto ToDto()
    {
        return new MedalSolutionDto(Text, SubmittedBy, Mods);
    }
}