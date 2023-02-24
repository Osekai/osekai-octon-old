using Microsoft.EntityFrameworkCore;
using Osekai.Octon.Domain.Enums;
using Osekai.Octon.Domain.Repositories;
using Osekai.Octon.Domain.ValueObjects;
using Osekai.Octon.Persistence.EntityFramework.MySql.Entities;
using BeatmapPack = Osekai.Octon.Domain.AggregateRoots.BeatmapPack;

namespace Osekai.Octon.Persistence.EntityFramework.MySql.Repositories;

public class MySqlEntityFrameworkMedalRepository: IMedalRepository
{
    protected MySqlOsekaiDbContext Context { get; }
    
    public MySqlEntityFrameworkMedalRepository(MySqlOsekaiDbContext context)
    {
        Context = context;
    }
    
    public async Task<IEnumerable<Domain.AggregateRoots.Medal>> GetMedalsAsync(
        IMedalRepository.MedalFilter filter = default,
        int offset  = 0, 
        int limit = int.MaxValue,
        bool includeBeatmapPacks = false,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Medal> query = Context.Medals
            .Include(e => e.Settings)
            .Include(e => e.Solution);
        
        if (includeBeatmapPacks)
            query.Include(e => e.BeatmapPacksForMedal)
                .ThenInclude(e => e.BeatmapPack);

        query = query.Take(limit)
            .Skip(offset);

        if (filter.Name != null)
            query = query.Where(medal => filter.Name.Contains(medal.Name));

        query = query.OrderBy(e => e.Id);
        
        Medal[] medals = await query.ToArrayAsync(cancellationToken);

        var packs = await Context.BeatmapPacksForMedals
            .Include(e => e.Medal)
            .Select(e => new { e.MedalId, e.BeatmapPack, e.Gamemode })
            .ToArrayAsync(cancellationToken);

        var packsByMedal = packs.GroupBy(
            e => e.MedalId, e => e).ToDictionary(e => e.Key, e => e.ToDictionary(l => l.Gamemode, l => l.BeatmapPack));

        return medals.Select(medal =>
        {
            Domain.AggregateRoots.Medal medalOut = medal.ToAggregateRoot();

            medalOut.FirstAchievement = new Ref<FirstAchievement?>(
                medal.FirstAchievedBy == null ? null :
                    new FirstAchievement(medal.FirstAchievedBy, medal.FirstAchievedDate!.Value)
            );

            medalOut.MedalSolution = medal.Solution?.ToValueObject();
            medalOut.MedalSettings = medal.Settings?.ToValueObject();

            if (includeBeatmapPacks && packsByMedal.TryGetValue(medalOut.Id, out var medalBeatmapPacks))
                medalOut.BeatmapPacks = medalBeatmapPacks.ToDictionary(e => e.Key, e => e.Value.ToAggregateRoot());
            else
                medalOut.BeatmapPacks = new Dictionary<OsuGamemode, BeatmapPack>();

            return medalOut;
        });
    }

    public async Task<IEnumerable<Domain.AggregateRoots.Medal>> GetMedalsByBeatmapPackIdAsync(int beatmapPackId, CancellationToken cancellationToken = default)
    {
        IEnumerable<Medal> medal = await Context.BeatmapPacksForMedals
            .Include(e => e.Medal)
            .ThenInclude(e => new { e.Settings, e.Solution })
            .Where(e => e.BeatmapPackId == beatmapPackId)
            .Select(e => e.Medal)
            .ToArrayAsync(cancellationToken);
        
        return medal.Select(m =>
        {
            Domain.AggregateRoots.Medal medalOut = m.ToAggregateRoot();
            
            medalOut.FirstAchievement =new Ref<FirstAchievement?>(
                    m.FirstAchievedBy == null ? null :
                        new FirstAchievement(m.FirstAchievedBy, m.FirstAchievedDate!.Value)
                );

            medalOut.MedalSolution = m.Solution?.ToValueObject();
            medalOut.MedalSettings = m.Settings?.ToValueObject();

            return medalOut;
        });
    }
}