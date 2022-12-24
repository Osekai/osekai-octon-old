namespace Osekai.Octon.Services.Query;

public readonly struct AuthenticationWithCodeQuery
{
    public AuthenticationWithCodeQuery(string code)
    {
        Code = code;
    }
    
    public string Code { get; }
}