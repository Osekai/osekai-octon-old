using Osekai.Octon.Domain.Aggregates;
using Osekai.Octon.Persistence;

namespace Osekai.Octon.Services;

public class AppService
{
    protected IUnitOfWork UnitOfWork { get; }
    
    public AppService(IUnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork;
    }

    public Task<App?> GetAppByIdAsync(int id, CancellationToken cancellationToken = default)
        => UnitOfWork.AppRepository.GetAppByIdAsync(id, cancellationToken);

    public Task<IEnumerable<App>> GetAppsAsync(CancellationToken cancellationToken = default) =>
        UnitOfWork.AppRepository.GetAppsAsync(cancellationToken);
}