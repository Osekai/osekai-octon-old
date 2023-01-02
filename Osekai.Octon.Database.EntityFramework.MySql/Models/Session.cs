using System.Text.Json;
using Osekai.Octon.Database.Dtos;
using Osekai.Octon.Database.HelperTypes;

namespace Osekai.Octon.Database.EntityFramework.MySql.Models;

internal sealed class Session
{
    public string Token { get; set; } = null!;
    public string Payload { get; set; } = null!;
    public DateTimeOffset ExpiresAt { get; set; }

    public SessionDto ToDto()
    {
        SessionDtoPayload payload = JsonSerializer.Deserialize<SessionDtoPayload>(Payload) ?? throw new InvalidDataException();
        return new SessionDto(Token, payload, ExpiresAt);
    }
}