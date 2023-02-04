using Osekai.Octon.Persistence.HelperTypes;

namespace Osekai.Octon.Objects;

public interface IReadOnlySession
{
    string Token { get; }
    SessionPayload Payload { get; }
    DateTimeOffset ExpiresAt { get; }
}