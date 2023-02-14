using Osekai.Octon.HelperTypes;

namespace Osekai.Octon.Models;

public interface IReadOnlySession
{
    string Token { get; }
    SessionPayload Payload { get; }
    DateTimeOffset ExpiresAt { get; }
}