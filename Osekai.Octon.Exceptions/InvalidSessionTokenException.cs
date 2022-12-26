namespace Osekai.Octon.Exceptions;

public class InvalidSessionTokenException: OsekaiException
{
    public InvalidSessionTokenException(string token)
    {
        Details = new { Token = token };
    }
    
    public override string ExceptionIdentifier => "invalidSessionToken";
    public override OsekaiExceptionReason Reason => OsekaiExceptionReason.Unauthorized;
    public override object Details { get; }
}