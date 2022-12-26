namespace Osekai.Octon.Exceptions;

public class NotAuthenticatedException: OsekaiException
{
    public override string ExceptionIdentifier => "notAuthenticated";
    public override OsekaiExceptionReason Reason => OsekaiExceptionReason.Unauthorized;
    public override object Details { get; } = new { };
}