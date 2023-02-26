using Osekai.Octon.Domain.AggregateRoots;

namespace Osekai.Octon.Domain.Services;

public interface ILocaleService
{
    Task<IEnumerable<Locale>> GetLocalesAsync(CancellationToken cancellationToken = default);
}