using Osekai.Octon.Exceptions;

namespace Osekai.Octon.Database.Repositories.Query;

public struct DeleteTokenQuery
{
    public DeleteTokenQuery(string token)
    {
        if (token.Length != Specifications.SessionTokenLength)
            throw new InvalidLengthException(nameof(Token), Specifications.SessionTokenLength);

        Token = token;
    }
    
    public string Token { get; }
}