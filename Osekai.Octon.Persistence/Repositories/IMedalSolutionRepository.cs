using Osekai.Octon.Persistence.Dtos;

namespace Osekai.Octon.Persistence.Repositories;

public interface IMedalSolutionRepository
{
    Task<MedalSolutionDto?> GetMedalSolutionByMedalIdAsync(int medalId, CancellationToken cancellationToken = default);
}