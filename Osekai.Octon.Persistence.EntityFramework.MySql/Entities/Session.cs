using System.Text.Json;
using Osekai.Octon.Domain.Repositories;
using Osekai.Octon.Domain.ValueObjects;

namespace Osekai.Octon.Persistence.EntityFramework.MySql.Entities;

internal sealed class Session
{
    public string Token { get; set; } = null!;
    public string Payload { get; set; } = null!;
    public DateTimeOffset ExpiresAt { get; set; }

    public Domain.AggregateRoots.Session ToAggregateRoot()
    {
        SessionPayload payload = JsonSerializer.Deserialize<SessionPayload>(Payload);
        return new Domain.AggregateRoots.Session(Token, payload, ExpiresAt);
    }
}