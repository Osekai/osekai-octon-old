using Osekai.Octon.Database.Models;
using Osekai.Octon.Database.Repositories.Query;

namespace Osekai.Octon.Database.Repositories;

public interface ICacheEntryRepository
{
    Task<CacheEntry?> GetCacheEntryFromNameAsync(GetCacheEntryFromNameQuery query, CancellationToken cancellationToken = default);
    Task<CacheEntry> AddOrUpdateCacheEntryAsync(AddOrUpdateCacheEntryQuery query, CancellationToken cancellationToken = default);
    Task DeleteCacheEntryAsync(DeleteCacheEntryQuery query, CancellationToken cancellationToken = default);

    Task<bool> CacheEntryExists(CacheEntryExistsQuery query, CancellationToken cancellationToken = default);
}