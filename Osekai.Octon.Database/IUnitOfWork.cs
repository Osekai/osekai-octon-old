using System.Data;
using Osekai.Octon.Database.Repositories;

namespace Osekai.Octon.Database;

public interface IUnitOfWork
{
    public IAppRepository AppRepository { get; }
    
    Task SaveAsync(CancellationToken cancellationToken = default);
}