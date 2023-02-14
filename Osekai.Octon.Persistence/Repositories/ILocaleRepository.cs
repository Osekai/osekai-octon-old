using Osekai.Octon.Models;

namespace Osekai.Octon.Persistence.Repositories;

public interface ILocaleRepository
{
    Task<IEnumerable<IReadOnlyLocale>> GetLocalesAsync(CancellationToken cancellationToken = default);
}