using System.Collections.Immutable;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Osekai.Octon.Database.Dtos;
using Osekai.Octon.Database.EntityFramework.MySql.Models;
using Osekai.Octon.Database.Repositories;

namespace Osekai.Octon.Database.EntityFramework.MySql.Repositories;

public class MySqlEntityFrameworkMedalRepository: IMedalRepository
{
    private readonly MySqlOsekaiDbContext _context;
    
    public MySqlEntityFrameworkMedalRepository(MySqlOsekaiDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<MedalDto>> GetMedalsAsync(
        IMedalRepository.MedalFilter filter = default,
        int offset  = 0, 
        int limit = int.MaxValue,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Medal> query = _context.Medals;

        query = query.Include(e => e.BeatmapPacksForMedal)
            .ThenInclude(e => e.BeatmapPack)
            .Include(e => e.Solution)
            .Include(e => e.Rarity)
            .Include(e => e.Settings)
            .Take(limit)
            .Skip(offset);

        if (filter.Name != null)
            query = query.Where(medal => filter.Name.Contains(medal.Name));

        IAsyncEnumerable<Medal> medals = query.AsAsyncEnumerable();

        return await medals.Select(medal => medal.ToDto()).ToArrayAsync();
    }
}