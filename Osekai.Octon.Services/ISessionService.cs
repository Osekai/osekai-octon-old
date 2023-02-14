using Osekai.Octon.HelperTypes;
using Osekai.Octon.Models;

namespace Osekai.Octon.Services;

public interface ISessionService
{
    Task RefreshSessionAsync(string token, CancellationToken cancellationToken);
    Task<SessionPayload> RefreshOsuSessionAsync(string token, CancellationToken cancellationToken);
}