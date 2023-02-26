using Osekai.Octon.Domain.AggregateRoots;

namespace Osekai.Octon.Domain.Services;

public interface IMedalService
{
    Task<IEnumerable<Medal>> GetMedalsAsync(bool includeBeatmapPacks = false, CancellationToken cancellationToken = default);
}