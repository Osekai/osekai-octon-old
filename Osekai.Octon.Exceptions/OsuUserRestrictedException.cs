namespace Osekai.Octon.Exceptions;

public class OsuUserRestrictedException: OsekaiException
{
    public OsuUserRestrictedException(int osuUserId)
    {
        Details = new { OsuUserId = osuUserId };
    }
    
    public override string ExceptionIdentifier => "userRestricted";
    public override OsekaiExceptionReason Reason => OsekaiExceptionReason.Forbidden;
    public override object Details { get; } 
}