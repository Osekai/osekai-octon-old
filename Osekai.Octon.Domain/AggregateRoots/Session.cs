using Osekai.Octon.Domain.Repositories;
using Osekai.Octon.Domain.ValueObjects;

namespace Osekai.Octon.Domain.AggregateRoots;

public class Session
{
    public Session(string token, DateTimeOffset expiresAt)
    {
        Token = token;
        ExpiresAt = expiresAt;
    }
    
    public string Token { get; } 
    public Ref<SessionPayload>? Payload { get; set; }
    public DateTimeOffset ExpiresAt { get; }
}