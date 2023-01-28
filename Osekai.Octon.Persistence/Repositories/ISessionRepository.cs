using Osekai.Octon.Persistence.Dtos;
using Osekai.Octon.Persistence.HelperTypes;

namespace Osekai.Octon.Persistence.Repositories;

public interface ISessionRepository
{
    Task<SessionDto?> GetSessionFromTokenAsync(string token, CancellationToken cancellationToken = default);
    Task<SessionDto> AddSessionAsync(string token, SessionDtoPayload payload, DateTimeOffset expiresAt, CancellationToken cancellationToken = default);
    Task<bool> UpdateSessionPayloadAsync(string token, SessionDtoPayload payload, CancellationToken cancellationToken = default);
    Task<bool> UpdateExpirationDateTimeAsync(string token, DateTimeOffset dateTime, CancellationToken cancellationToken = default);
    Task<bool> SessionExists(string token, CancellationToken cancellationToken = default);
    Task DeleteSessionAsync(string token, CancellationToken cancellationToken = default);
}