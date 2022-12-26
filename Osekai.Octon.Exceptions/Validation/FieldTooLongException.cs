﻿namespace Osekai.Octon.Exceptions;

public class FieldTooLongException: ValidationException
{
    public FieldTooLongException(string fieldIdentifier, int maxLength, int actualLength) 
        : base(
            fieldIdentifier, 
            new
            {
                MaxLength = maxLength,
                ActualLength = actualLength
            })
    {
    }

    public override string ValidationExceptionIdentifier => "fieldTooLong";
}