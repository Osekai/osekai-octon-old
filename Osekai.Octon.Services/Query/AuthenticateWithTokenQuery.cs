using Osekai.Octon.Database;
using Osekai.Octon.Exceptions;

namespace Osekai.Octon.Services.Query;

public readonly struct AuthenticateWithTokenQuery
{
    public AuthenticateWithTokenQuery(string token)
    {
        if (token.Length != Specifications.SessionTokenLength)
            throw new InvalidLengthException(nameof(Token), Specifications.SessionTokenLength);
        
        Token = token;
    }
    
    public string Token { get; }
}