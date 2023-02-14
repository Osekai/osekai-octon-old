using Osekai.Octon.HelperTypes;
using Osekai.Octon.Models;

namespace Osekai.Octon.Persistence.EntityFramework.MySql.Dtos;

internal sealed class SessionDto: IReadOnlySession
{
    public SessionDto(string token, SessionPayload payload, DateTimeOffset expiresAt)
    {
        Token = token;
        Payload = (SessionPayload) payload.Clone();
        ExpiresAt = expiresAt;
    }
    
    public string Token { get; }
    public SessionPayload Payload { get; }
    public DateTimeOffset ExpiresAt { get; }
}