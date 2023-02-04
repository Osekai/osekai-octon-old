using Osekai.Octon.Persistence;
using Osekai.Octon.Services.Entities;
using Osekai.Octon.Services.Extensions;

namespace Osekai.Octon.Services;

public class AppService
{
    protected IUnitOfWork UnitOfWork { get; }
    
    public AppService(IUnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork;
    }

    public Task<App?> GetAppByIdAsync(int id, CancellationToken cancellationToken = default)
        => UnitOfWork.AppRepository.GetAppByIdAsync(id, cancellationToken)
            .ContinueWith(t => t.Result?.ToEntity(UnitOfWork));

    public Task<IEnumerable<App>> GetAppsAsync(CancellationToken cancellationToken = default) =>
        UnitOfWork.AppRepository.GetAppsAsync(cancellationToken)
            .ContinueWith(t => t.Result.Select(d => d.ToEntity(UnitOfWork)));
}