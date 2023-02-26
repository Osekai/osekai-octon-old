using Osekai.Octon.Domain.AggregateRoots;
using Osekai.Octon.Domain.Repositories;
using Osekai.Octon.Domain.Services;
using Osekai.Octon.Persistence;

namespace Osekai.Octon.Domain.Services.Default;

public class MedalService : IMedalService
{
    private IUnitOfWork UnitOfWork { get; }
    
    public MedalService(IUnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork;
    }

    public Task<IEnumerable<Medal>> GetMedalsAsync(bool includeBeatmapPacks = false, CancellationToken cancellationToken = default)
        => UnitOfWork.MedalRepository.GetMedalsAsync(includeBeatmapPacks: includeBeatmapPacks, cancellationToken: cancellationToken);
}