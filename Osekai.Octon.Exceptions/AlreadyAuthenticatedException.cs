namespace Osekai.Octon.Exceptions;

public class AlreadyAuthenticatedException: OsekaiException
{
    public override string ExceptionIdentifier => "alreadyAuthenticated";
    public override OsekaiExceptionReason Reason => OsekaiExceptionReason.BadInput;
    public override object Details { get; } = new { };
}