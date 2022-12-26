namespace Osekai.Octon.Exceptions;

public class InvalidSessionTokenFormatException: OsekaiException
{
    public InvalidSessionTokenFormatException(string token)
    {
        Details = token;
    }
    
    public override string ExceptionIdentifier => "invalidSessionTokenFormat";
    public override OsekaiExceptionReason Reason => OsekaiExceptionReason.BadInput;
    
    public override object Details { get; }
}