using Osekai.Octon.Database.HelperTypes;

namespace Osekai.Octon.Database.Dtos;

public class SessionDto: ICloneable
{
    public SessionDto(string token, SessionDtoPayload payload, DateTimeOffset expiresAt)
    {
        Token = token;
        Payload = (SessionDtoPayload) payload.Clone();
        ExpiresAt = expiresAt;
    }
    
    public string Token { get; }
    public SessionDtoPayload Payload { get; }
    public DateTimeOffset ExpiresAt { get; }

    public object Clone()
    {
        return new SessionDto(Token, (SessionDtoPayload)Payload.Clone(), ExpiresAt);
    }
}