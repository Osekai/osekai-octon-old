using Osekai.Octon.Domain.AggregateRoots;

namespace Osekai.Octon.Domain.Repositories;

public interface ISessionRepository
{
    Task<Session?> GetSessionByTokenAsync(string token, CancellationToken cancellationToken = default);
    Task AddSessionAsync(Session session, CancellationToken cancellationToken = default);
    Task<bool> SaveSessionAsyncAsync(Session session, CancellationToken cancellationToken = default);
    Task<bool> SessionExistsByToken(string token, CancellationToken cancellationToken = default);
    Task DeleteSessionAsync(Session session, CancellationToken cancellationToken = default);
    Task DeleteSessionByTokenAsync(string token, CancellationToken cancellationToken = default);

}