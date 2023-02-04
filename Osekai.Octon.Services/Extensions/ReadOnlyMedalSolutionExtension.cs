using Osekai.Octon.Objects;
using Osekai.Octon.Persistence.Repositories;
using Osekai.Octon.Services.Entities;

namespace Osekai.Octon.Services.Extensions;

internal static class ReadOnlyMedalSolutionExtension
{
    internal static MedalSolution ToEntity(this IReadOnlyMedalSolution medalSolution)
    {
        return new MedalSolution(
            medalSolution.Text, 
            medalSolution.SubmittedBy, 
            medalSolution.Mods
        );
    }
}