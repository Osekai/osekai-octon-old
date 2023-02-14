using System.Text.Json;
using Osekai.Octon.HelperTypes;
using Osekai.Octon.Models;
using Osekai.Octon.Persistence.EntityFramework.MySql.Dtos;

namespace Osekai.Octon.Persistence.EntityFramework.MySql.Entities;

internal sealed class Session
{
    public string Token { get; set; } = null!;
    public string Payload { get; set; } = null!;
    public DateTimeOffset ExpiresAt { get; set; }

    public SessionDto ToDto()
    {
        SessionPayload payload = JsonSerializer.Deserialize<SessionPayload>(Payload) ?? throw new InvalidDataException();
        return new SessionDto(Token, payload, ExpiresAt);
    }
}