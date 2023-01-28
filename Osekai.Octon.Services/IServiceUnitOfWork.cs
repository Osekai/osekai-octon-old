using Osekai.Octon.Persistence;

namespace Osekai.Octon.Services;

public class ServiceUnitOfWork
{
    protected IDatabaseUnitOfWork UnitOfWork { get; }
    
    public ServiceUnitOfWork(IDatabaseUnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
        UnitOfWork.SaveChangesAsync(cancellationToken);
}