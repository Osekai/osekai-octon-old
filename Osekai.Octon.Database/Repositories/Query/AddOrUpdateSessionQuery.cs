using System.Text.Json;
using Osekai.Octon.Database.HelperTypes;
using Osekai.Octon.Exceptions;

namespace Osekai.Octon.Database.Repositories.Query;

public struct AddOrUpdateSessionQuery
{
    public AddOrUpdateSessionQuery(string token, string payload)
    {
        if (token.Length != Specifications.SessionTokenLength)
            throw new InvalidLengthException(nameof(Token), Specifications.SessionTokenLength);
        
        Token = token;
        Payload = payload;
    }
    
    public AddOrUpdateSessionQuery(string token, SessionPayload payload) : this(token, JsonSerializer.Serialize(payload))
    {
    }
    
    public string Token { get; }
    public string Payload { get; }
}