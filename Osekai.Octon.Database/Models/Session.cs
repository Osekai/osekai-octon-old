namespace Osekai.Octon.Database.Models;

public sealed class Session: ICloneable
{
    public string Token { get; set; } = null!;
    public string Payload { get; set; } = null!;
    public DateTimeOffset ExpiresAt { get; set; }
    
    public object Clone()
    {
        return new Session
        {
            Payload = Payload,
            Token = Token,
            ExpiresAt = ExpiresAt
        };
    }
}