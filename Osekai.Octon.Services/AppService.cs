using Osekai.Octon.Persistence;
using Osekai.Octon.Services.Entities;
using Osekai.Octon.Services.Extensions;

namespace Osekai.Octon.Services;

public class AppService
{
    protected IDatabaseUnitOfWork UnitOfWork { get; }
    
    public AppService(IDatabaseUnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork;
    }

    public Task<App?> GetAppByIdAsync(int id, CancellationToken cancellationToken = default)
        => UnitOfWork.AppRepository.GetAppByIdAsync(id)
            .ContinueWith(t => t.Result?.ToEntity(UnitOfWork));
    
    
}