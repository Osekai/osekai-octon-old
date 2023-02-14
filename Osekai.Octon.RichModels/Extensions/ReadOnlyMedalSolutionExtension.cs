using Osekai.Octon.Models;

namespace Osekai.Octon.RichModels.Extensions;

internal static class ReadOnlyMedalSolutionExtension
{
    public static MedalSolution ToRichModel(this IReadOnlyMedalSolution medalSolution)
    {
        return new MedalSolution(
            medalSolution.Text, 
            medalSolution.SubmittedBy, 
            medalSolution.Mods
        );
    }
}