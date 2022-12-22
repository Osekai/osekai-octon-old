using Microsoft.EntityFrameworkCore;
using Osekai.Octon.Database.Repositories;

namespace Osekai.Octon.Database.EntityFramework;

public class EntityFrameworkTransactionalUnitOfWork: ITransactionalUnitOfWork
{
    private readonly ITransaction _transaction;
    private readonly IUnitOfWork _unitOfWork;
    
    public EntityFrameworkTransactionalUnitOfWork(IUnitOfWork unitOfWork, ITransaction transaction)
    {
        _unitOfWork = unitOfWork;
        _transaction = transaction;
    }

    public IAppRepository AppRepository => _unitOfWork.AppRepository;

    public Task SaveAsync(CancellationToken cancellationToken) => _unitOfWork.SaveAsync(cancellationToken);

    public ValueTask DisposeAsync() => _transaction.DisposeAsync();
    public void Dispose() => _transaction.Dispose();

    public Task CommitAsync(CancellationToken cancellationToken = default) =>
        _transaction.CommitAsync(cancellationToken);

    public Task RollbackAsync(CancellationToken cancellationToken = default) =>
        _transaction.RollbackAsync(cancellationToken);
}