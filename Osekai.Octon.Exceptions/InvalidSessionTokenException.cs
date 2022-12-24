namespace Osekai.Octon.Exceptions;

public class InvalidSessionTokenException: OsekaiException
{
    public InvalidSessionTokenException(string token)
    {
        Details = token;
    }
    
    public override string ExceptionIdentifier => "invalidSessionToken";
    public override OsekaiExceptionReason Reason => OsekaiExceptionReason.BadInput;
    
    public override object Details { get; }
}