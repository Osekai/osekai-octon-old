using Osekai.Octon.Models;

namespace Osekai.Octon.Persistence.Repositories;

public interface IMedalSolutionRepository
{
    Task<IReadOnlyMedalSolution?> GetMedalSolutionByMedalIdAsync(int medalId, CancellationToken cancellationToken = default);
}