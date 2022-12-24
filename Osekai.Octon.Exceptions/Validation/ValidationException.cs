using System.Reflection.Metadata;

namespace Osekai.Octon.Exceptions;

public abstract class ValidationException: OsekaiException
{
    protected ValidationException(string fieldIdentifier, object validationInfo)
    {
        Details = new
        {
            FieldIdentifier = fieldIdentifier,
            ValidationInfo = validationInfo
        };
    }
    
    public override string ExceptionIdentifier => "validationError";
    public override OsekaiExceptionReason Reason => OsekaiExceptionReason.BadInput;
    public override object Details { get; }
}