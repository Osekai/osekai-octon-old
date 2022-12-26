namespace Osekai.Octon.Exceptions;

public class FieldTooShortException: ValidationException
{
    public FieldTooShortException(string fieldIdentifier, int minLength, int actualLength) 
        : base(fieldIdentifier, 
            new
            {
                MinLength = minLength,
                ActualLength = actualLength
            })
    {
    }

    public override string ValidationExceptionIdentifier => "fieldTooShort";
}