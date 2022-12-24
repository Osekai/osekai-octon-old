namespace Osekai.Octon.Exceptions;

public abstract class OsekaiException: Exception
{
    public override string Message => ExceptionIdentifier;

    public abstract string ExceptionIdentifier { get; }
    public abstract OsekaiExceptionReason Reason { get; }
    public abstract object Details { get; }
}