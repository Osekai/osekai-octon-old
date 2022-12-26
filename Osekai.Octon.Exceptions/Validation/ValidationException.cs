using System.Reflection.Metadata;
using System.Text.Json;

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
    
    public abstract string ValidationExceptionIdentifier { get; }
    
    public override string Message => JsonSerializer.Serialize(new
    {
        ExceptionIdentifier,
        ValidationExceptionIdentifier,
        Reason,
        Details
    });
    
    public override OsekaiExceptionReason Reason => OsekaiExceptionReason.BadInput;
    public override object Details { get; }
}