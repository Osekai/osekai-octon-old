using Osekai.Octon.Domain.AggregateRoots;
using Osekai.Octon.Domain.Services;
using Osekai.Octon.Persistence;

namespace Osekai.Octon.Domain.Services.Default;


public class AppService : IAppService
{
    protected IUnitOfWork UnitOfWork { get; }
    
    public AppService(IUnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork;
    }

    public Task<App?> GetAppByIdAsync(int id, bool includeFaqs = false, CancellationToken cancellationToken = default)
        => UnitOfWork.AppRepository.GetAppByIdAsync(id, includeFaqs, cancellationToken);

    public Task<IEnumerable<App>> GetAppsAsync(bool includeFaqs = false, CancellationToken cancellationToken = default) =>
        UnitOfWork.AppRepository.GetAppsAsync(includeFaqs, cancellationToken);
}