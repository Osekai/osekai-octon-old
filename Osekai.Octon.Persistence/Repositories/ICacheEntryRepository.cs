﻿using Osekai.Octon.Persistence.Dtos;

namespace Osekai.Octon.Persistence.Repositories;

public interface ICacheEntryRepository
{
    Task<CacheEntryDto?> GetCacheEntryByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<CacheEntryDto> AddOrUpdateCacheEntryAsync(string name, byte[] data, DateTimeOffset expiresAfter, CancellationToken cancellationToken = default);
    Task DeleteCacheEntryAsync(string name, CancellationToken cancellationToken = default);
    Task<bool> CacheEntryExists(string name, CancellationToken cancellationToken = default);
}