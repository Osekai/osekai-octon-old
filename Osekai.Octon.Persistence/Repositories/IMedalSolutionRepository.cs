using Osekai.Octon.Objects;

namespace Osekai.Octon.Persistence.Repositories;

public interface IMedalSolutionRepository
{
    Task<IReadOnlyMedalSolution?> GetMedalSolutionByMedalIdAsync(int medalId, CancellationToken cancellationToken = default);
}