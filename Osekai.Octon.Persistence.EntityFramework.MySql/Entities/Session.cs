using System.Text.Json;
using Osekai.Octon.Domain.Entities;

namespace Osekai.Octon.Persistence.EntityFramework.MySql.Entities;

internal sealed class Session
{
    public string Token { get; set; } = null!;
    public string Payload { get; set; } = null!;
    public DateTimeOffset ExpiresAt { get; set; }

    public Domain.Aggregates.Session ToAggregate()
    {
        SessionPayload payload = JsonSerializer.Deserialize<SessionPayload>(Payload) ?? throw new InvalidDataException();
        return new Domain.Aggregates.Session(Token, payload, ExpiresAt);
    }
}