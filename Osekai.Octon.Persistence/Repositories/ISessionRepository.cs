using Osekai.Octon.HelperTypes;
using Osekai.Octon.Models;

namespace Osekai.Octon.Persistence.Repositories;

public interface ISessionRepository
{
    Task<IReadOnlySession?> GetSessionByTokenAsync(string token, CancellationToken cancellationToken = default);
    Task AddSessionAsync(IReadOnlySession session, CancellationToken cancellationToken = default);
    Task<bool> SaveSessionAsyncAsync(IReadOnlySession session, CancellationToken cancellationToken = default);
    Task<bool> SessionExistsByToken(string token, CancellationToken cancellationToken = default);
    Task DeleteSessionAsync(IReadOnlySession session, CancellationToken cancellationToken = default);
    Task DeleteSessionByTokenAsync(string token, CancellationToken cancellationToken = default);

}