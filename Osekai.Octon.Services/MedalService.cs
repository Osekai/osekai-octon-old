using Osekai.Octon.Domain.AggregateRoots;
using Osekai.Octon.Domain.Repositories;
using Osekai.Octon.Persistence;

namespace Osekai.Octon.Services;

public class MedalService
{
    private IUnitOfWork UnitOfWork { get; }
    
    public MedalService(IUnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork;
    }

    public Task<IEnumerable<Medal>> GetMedalsAsync(bool includeBeatmapPacks = false, CancellationToken cancellationToken = default)
        => UnitOfWork.MedalRepository.GetMedalsAsync(includeBeatmapPacks: includeBeatmapPacks, cancellationToken: cancellationToken);
}