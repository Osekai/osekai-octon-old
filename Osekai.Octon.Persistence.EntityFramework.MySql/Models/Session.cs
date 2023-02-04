using System.Text.Json;
using Osekai.Octon.Persistence.EntityFramework.MySql.Dtos;
using Osekai.Octon.Persistence.HelperTypes;

namespace Osekai.Octon.Persistence.EntityFramework.MySql.Models;

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