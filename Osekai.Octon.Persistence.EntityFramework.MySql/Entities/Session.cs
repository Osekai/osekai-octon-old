using System.Text.Json;
using Osekai.Octon.Domain.Repositories;
using Osekai.Octon.Domain.ValueObjects;
using Osekai.Octon.Persistence.EntityFramework.MySql.Serializables;

namespace Osekai.Octon.Persistence.EntityFramework.MySql.Entities;

internal sealed class Session
{
    public string Token { get; set; } = null!;
    public SerializableSessionPayload Payload { get; set; }
    public DateTimeOffset ExpiresAt { get; set; }

    public Domain.AggregateRoots.Session ToAggregateRoot()
    {
        return new Domain.AggregateRoots.Session(Token, ExpiresAt);
    }
}