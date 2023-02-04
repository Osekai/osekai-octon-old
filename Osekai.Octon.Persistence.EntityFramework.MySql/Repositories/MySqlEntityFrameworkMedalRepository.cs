using Microsoft.EntityFrameworkCore;
using Osekai.Octon.Objects;
using Osekai.Octon.Persistence.EntityFramework.MySql.Dtos;
using Osekai.Octon.Persistence.EntityFramework.MySql.Models;
using Osekai.Octon.Persistence.Repositories;

namespace Osekai.Octon.Persistence.EntityFramework.MySql.Repositories;

public class MySqlEntityFrameworkMedalRepository: IMedalRepository
{
    protected MySqlOsekaiDbContext Context { get; }
    
    public MySqlEntityFrameworkMedalRepository(MySqlOsekaiDbContext context)
    {
        Context = context;
    }
    
    public async Task<IEnumerable<IReadOnlyMedal>> GetMedalsAsync(
        IMedalRepository.MedalFilter filter = default,
        int offset  = 0, 
        int limit = int.MaxValue,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Medal> query = Context.Medals
            .Take(limit)
            .Skip(offset);

        if (filter.Name != null)
            query = query.Where(medal => filter.Name.Contains(medal.Name));
        
        IAsyncEnumerable<Medal> medals = query.AsAsyncEnumerable();

        return await medals.Select(medal => medal.ToDto()).ToArrayAsync(cancellationToken);
    }

    public async Task<IEnumerable<IReadOnlyMedal>> GetMedalsByBeatmapPackIdAsync(int beatmapPackId, CancellationToken cancellationToken = default)
    {
        IEnumerable<Medal> medal = await Context.BeatmapPacksForMedals
            .Include(e => e.Medal)
            .Where(e => e.BeatmapPackId == beatmapPackId)
            .Select(e => e.Medal)
            .ToArrayAsync(cancellationToken);
        
        return medal.Select(m => m.ToDto());
    }
}