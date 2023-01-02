using Microsoft.EntityFrameworkCore;
using Osekai.Octon.Database.Dtos;
using Osekai.Octon.Database.EntityFramework.MySql.Models;
using Osekai.Octon.Database.Repositories;

namespace Osekai.Octon.Database.EntityFramework.MySql.Repositories;

public class MySqlEntityFrameworkCacheEntryRepository: ICacheEntryRepository
{
    private readonly MySqlOsekaiDbContext _context;
    
    public MySqlEntityFrameworkCacheEntryRepository(MySqlOsekaiDbContext context)
    {
        _context = context;
    }

    public async Task<CacheEntryDto?> GetCacheEntryByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        CacheEntry? entry = await _context.CacheEntries.AsNoTracking().Where(c => c.Name == name).FirstOrDefaultAsync(cancellationToken);
        return entry?.ToDto();
    }

    public async Task<CacheEntryDto> AddOrUpdateCacheEntryAsync(string name, byte[] data, DateTimeOffset expiresAt, CancellationToken cancellationToken = default)
    {
        CacheEntry? cacheEntry = await _context.CacheEntries.Where(c => c.Name == name).FirstOrDefaultAsync(cancellationToken);

        if (cacheEntry != null)
        {
            cacheEntry.Data = data;
            cacheEntry.ExpiresAt = expiresAt;
        }
        else
            _context.Add(
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
        CacheEntry? cacheEntry = await _context.CacheEntries.Where(c => c.Name == name).FirstOrDefaultAsync(cancellationToken);
        if (cacheEntry != null)
            _context.Remove(cacheEntry);
    }

    public Task<bool> CacheEntryExists(string name, CancellationToken cancellationToken = default)
    {
        return _context.CacheEntries.AsNoTracking().AnyAsync(c => c.Name == name);
    }
}