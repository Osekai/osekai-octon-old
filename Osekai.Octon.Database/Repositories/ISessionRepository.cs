using Osekai.Octon.Database.Dtos;
using Osekai.Octon.Database.HelperTypes;

namespace Osekai.Octon.Database.Repositories;

public interface ISessionRepository
{
    Task<SessionDto?> GetSessionFromTokenAsync(string token, CancellationToken cancellationToken = default);
    Task<SessionDto> AddSessionAsync(string token, SessionDtoPayload payload, DateTimeOffset expiresAt, CancellationToken cancellationToken = default);
    Task<bool> UpdateSessionPayloadAsync(string token, SessionDtoPayload payload, CancellationToken cancellationToken = default);
    Task<bool> UpdateExpirationDateTimeAsync(string token, DateTimeOffset dateTime, CancellationToken cancellationToken = default);
    Task<bool> SessionExists(string token, CancellationToken cancellationToken = default);
    Task DeleteSessionAsync(string token, CancellationToken cancellationToken = default);
}