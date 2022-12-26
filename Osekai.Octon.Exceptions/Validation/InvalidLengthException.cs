namespace Osekai.Octon.Exceptions;

public class InvalidLengthException: ValidationException
{
    public InvalidLengthException(string fieldIdentifier, int length) : base(fieldIdentifier, new { Length = length })
    {
    }

    public override string ValidationExceptionIdentifier => "invalidLength";
}