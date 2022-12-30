namespace Osekai.Octon.Exceptions;

public class OsuUserDeletedException: OsekaiException
{
    public OsuUserDeletedException(int osuUserId)
    {
        Details = new { OsuUserId = osuUserId };
    }
    
    public override string ExceptionIdentifier => "userDeleted";
    public override OsekaiExceptionReason Reason => OsekaiExceptionReason.Forbidden;
    public override object Details { get; }
}