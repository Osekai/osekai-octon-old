﻿using Osekai.Octon.Database.Models;
using Osekai.Octon.Exceptions;

namespace Osekai.Octon.Database.Repositories.Query;

public readonly struct CacheEntryExistsQuery 
{
    public CacheEntryExistsQuery(string name)
    {
        if (name.Length > Specifications.CacheEntryNameMaxLength)
            throw new FieldTooLongException(nameof(Name), Specifications.CacheEntryNameMaxLength, name.Length);
        
        if (name.Length < Specifications.CacheEntryNameMinLength)
            throw new FieldTooShortException(nameof(Name), Specifications.CacheEntryNameMinLength, name.Length);

        Name = name;
    }
    
    public string Name { get; }
}