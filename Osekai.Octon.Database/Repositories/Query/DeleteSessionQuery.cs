using Osekai.Octon.Exceptions;
using Osekai.Octon.Exceptions.Validation;

namespace Osekai.Octon.Database.Repositories.Query;

public struct DeleteSessionQuery
{
    public DeleteSessionQuery(string token)
    {
        if (token.Length != Specifications.SessionTokenLength)
            throw new InvalidLengthException(nameof(Token), Specifications.SessionTokenLength);

        Token = token;
    }
    
    public string Token { get; }
}