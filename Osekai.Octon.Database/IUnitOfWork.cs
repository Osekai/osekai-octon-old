using System.Data;
using Osekai.Octon.Database.Repositories;

namespace Osekai.Octon.Database;

public interface IUnitOfWork
{
    public IAppRepository AppRepository { get; }
    public ISessionRepository SessionRepository { get; }
    
    Task SaveAsync(CancellationToken cancellationToken = default);
}