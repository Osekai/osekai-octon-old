using System.Collections.Immutable;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Osekai.Octon.Database.Models;
using Osekai.Octon.Database.Repositories;

namespace Osekai.Octon.Database.EntityFramework.MySql.Repositories;

public class MySqlEntityFrameworkMedalRepository: IMedalRepository
{
    private readonly MySqlOsekaiDbContext _context;
    
    public MySqlEntityFrameworkMedalRepository(MySqlOsekaiDbContext context)
    {
        _context = context;
    }
    
    public async Task<IReadOnlyCollection<Medal>> GetMedalsAsync(
        Expression<Func<Medal, bool>>? filter = null, 
        long offset  = 0, 
        long limit = long.MaxValue,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Medal> query = _context.Medals;

        if (filter != null)
            query = query.Where(filter);

        return await query.Include(e => e.BeatmapPacksForMedal)
            .ThenInclude(e => e.BeatmapPack)
            .ToArrayAsync(cancellationToken);
    }
}