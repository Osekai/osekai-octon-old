using Osekai.Octon.Domain.AggregateRoots;

namespace Osekai.Octon.Domain.Repositories;

public interface ILocaleRepository
{
    Task<IEnumerable<Locale>> GetLocalesAsync(CancellationToken cancellationToken = default);
}