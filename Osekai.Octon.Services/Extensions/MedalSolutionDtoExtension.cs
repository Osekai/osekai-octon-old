using Osekai.Octon.Persistence.Dtos;
using Osekai.Octon.Services.Entities;

namespace Osekai.Octon.Services.Extensions;

internal static class MedalSolutionDtoExtension
{
    internal static MedalSolution ToEntity(this MedalSolutionDto medalSolutionDto)
    {
        return new MedalSolution(
            medalSolutionDto.Text, 
            medalSolutionDto.SubmittedBy, 
            medalSolutionDto.Mods
        );
    }
}