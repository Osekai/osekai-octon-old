using Microsoft.EntityFrameworkCore;
using Osekai.Octon.Database.Models;
using Osekai.Octon.Database.Repositories;
using Osekai.Octon.Database.Repositories.Query;

namespace Osekai.Octon.Database.EntityFramework.MySql.Repositories;

public class MySqlEntityFrameworkCacheEntryRepository: ICacheEntryRepository
{
    private readonly MySqlOsekaiDbContext _context;
    
    public MySqlEntityFrameworkCacheEntryRepository(MySqlOsekaiDbContext context)
    {
        _context = context;
    }

    public Task<CacheEntry?> GetCacheEntryFromNameAsync(GetCacheEntryFromNameQuery query, CancellationToken cancellationToken = default)
    {
        return _context.CacheEntries.AsNoTracking().Where(c => c.Name == query.Name).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<CacheEntry> AddOrUpdateCacheEntryAsync(AddOrUpdateCacheEntryQuery query, CancellationToken cancellationToken = default)
    {
        if (await CacheEntryExists(new CacheEntryExistsQuery(query.Name), cancellationToken))
            _context.Entry(
                new CacheEntry()
                {
                    Name = query.Name,
                    Data = query.Data,
                    ExpiresAt = query.ExpiresAt
                }).State = EntityState.Modified;
        else
            _context.Add(
                new CacheEntry()
                {
                    Name = query.Name, 
                    Data = query.Data,
                    ExpiresAt = query.ExpiresAt
                });

        return new CacheEntry()
        {
            Name = query.Name, 
            Data = query.Data,
            ExpiresAt = query.ExpiresAt
        };
    }

    public Task DeleteCacheEntryAsync(DeleteCacheEntryQuery query, CancellationToken cancellationToken = default)
    {
        _context.Entry(new CacheEntry() { Name = query.Name }).State = EntityState.Deleted;
        return Task.CompletedTask;
    }

    public Task<bool> CacheEntryExists(CacheEntryExistsQuery query, CancellationToken cancellationToken = default)
    {
        return _context.CacheEntries.AsNoTracking().AnyAsync(c => c.Name == query.Name);
    }
}