using System.Text.Json;

namespace Osekai.Octon.Exceptions;

public abstract class OsekaiException: Exception
{
    public override string Message => JsonSerializer.Serialize(new
    {
        ExceptionIdentifier,
        Reason,
        Details
    });

    public abstract string ExceptionIdentifier { get; }
    public abstract OsekaiExceptionReason Reason { get; }
    public abstract object Details { get; }
}