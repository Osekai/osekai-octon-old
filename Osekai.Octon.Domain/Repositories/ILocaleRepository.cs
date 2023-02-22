using Osekai.Octon.Domain.Aggregates;
using Osekai.Octon.Domain.Entities;

namespace Osekai.Octon.Domain.Repositories;

public interface ILocaleRepository
{
    Task<IEnumerable<Locale>> GetLocalesAsync(CancellationToken cancellationToken = default);
}