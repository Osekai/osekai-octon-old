using Osekai.Octon.Domain.Entities;

namespace Osekai.Octon.Domain.Aggregates;

public class Session
{
    public Session(string token, SessionPayload payload, DateTimeOffset expiresAt)
    {
        Token = token;
        Payload = payload;
        ExpiresAt = expiresAt;
    }
    
    public string Token { get; } 
    public SessionPayload Payload { get; set; }
    public DateTimeOffset ExpiresAt { get; }
}