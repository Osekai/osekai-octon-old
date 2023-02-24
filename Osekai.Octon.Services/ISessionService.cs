using Osekai.Octon.Domain.Repositories;
using Osekai.Octon.Domain.ValueObjects;

namespace Osekai.Octon.Services;

public interface ISessionService
{
    Task RefreshSessionAsync(string token, CancellationToken cancellationToken);
    Task<SessionPayload> RefreshOsuSessionAsync(string token, CancellationToken cancellationToken);
}