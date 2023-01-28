using Microsoft.EntityFrameworkCore;
using Osekai.Octon.Persistence.Dtos;
using Osekai.Octon.Persistence.EntityFramework.MySql.Models;
using Osekai.Octon.Persistence.Repositories;

namespace Osekai.Octon.Persistence.EntityFramework.MySql.Repositories;

public class MySqlEntityFrameworkCacheEntryRepository: ICacheEntryRepository
{
    protected MySqlOsekaiDbContext Context { get; }
    
    public MySqlEntityFrameworkCacheEntryRepository(MySqlOsekaiDbContext context)
    {
        Context = context;
    }

    public async Task<CacheEntryDto?> GetCacheEntryByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        CacheEntry? entry = await Context.CacheEntries.FindAsync(new object[] {name}, cancellationToken);
        return entry?.ToDto();
    }

    public async Task<CacheEntryDto> AddOrUpdateCacheEntryAsync(string name, byte[] data, DateTimeOffset expiresAt, CancellationToken cancellationToken = default)
    {
        CacheEntry? cacheEntry = await Context.CacheEntries.FindAsync(new object[] {name}, cancellationToken);

        if (cacheEntry != null)
        {
            cacheEntry.Data = data;
            cacheEntry.ExpiresAt = expiresAt;
        }
        else
            Context.Add(
                new CacheEntry()
                {
                    Name = name, 
                    Data = data,
                    ExpiresAt = expiresAt
                });

        return new CacheEntryDto(name, data, expiresAt);
    }

    public async Task DeleteCacheEntryAsync(string name, CancellationToken cancellationToken = default)
    {
        CacheEntry? cacheEntry = await Context.CacheEntries.FindAsync(new object[] {name}, cancellationToken);
        if (cacheEntry != null)
            Context.Remove(cacheEntry);
    }

    public async Task<bool> CacheEntryExists(string name, CancellationToken cancellationToken = default)
    {
        return await Context.CacheEntries.FindAsync(new object[] {name}, cancellationToken) != null;
    }
}