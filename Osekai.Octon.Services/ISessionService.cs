using Osekai.Octon.Domain.Entities;

namespace Osekai.Octon.Services;

public interface ISessionService
{
    Task RefreshSessionAsync(string token, CancellationToken cancellationToken);
    Task<SessionPayload> RefreshOsuSessionAsync(string token, CancellationToken cancellationToken);
}