using Osekai.Octon.Persistence;
using Osekai.Octon.Persistence.Dtos;
using Osekai.Octon.Persistence.Repositories;
using Osekai.Octon.Services.Extensions;

namespace Osekai.Octon.Services.Entities;

public class BeatmapPack
{
    protected internal IUnitOfWork UnitOfWork { get; }
    
    protected internal BeatmapPack(int id, int beatmapCount, IUnitOfWork unitOfWork)
    {
        Id = id;
        BeatmapCount = beatmapCount;
        UnitOfWork = unitOfWork;
    }
    
    public int Id { get; }
    public int BeatmapCount { get; set; }

    public async Task<IEnumerable<Medal>> GetMedalsAsync(CancellationToken cancellationToken)
    {
        IEnumerable<MedalDto> medals = await UnitOfWork.MedalRepository.GetMedalsAsync(
            new IMedalRepository.MedalFilter(beatmapPackIds: new int[] { Id }), cancellationToken: cancellationToken);
        
        return medals.Select(m => m.ToEntity(UnitOfWork));
    }
}