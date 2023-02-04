using Osekai.Octon.Objects;
using Osekai.Octon.Persistence.HelperTypes;

namespace Osekai.Octon.Persistence.Repositories;

public interface ISessionRepository
{
    Task<IReadOnlySession?> GetSessionFromTokenAsync(string token, CancellationToken cancellationToken = default);
    Task<IReadOnlySession> AddSessionAsync(string token, SessionPayload payload, DateTimeOffset expiresAt, CancellationToken cancellationToken = default);
    Task<bool> UpdateSessionPayloadAsync(string token, SessionPayload payload, CancellationToken cancellationToken = default);
    Task<bool> UpdateExpirationDateTimeAsync(string token, DateTimeOffset dateTime, CancellationToken cancellationToken = default);
    Task<bool> SessionExists(string token, CancellationToken cancellationToken = default);
    Task DeleteSessionAsync(string token, CancellationToken cancellationToken = default);
}