using Osekai.Octon.Exceptions;
using Osekai.Octon.Exceptions.Validation;

namespace Osekai.Octon.Database.Repositories.Query;

public readonly struct DeleteCacheEntryQuery
{
    public DeleteCacheEntryQuery(string name)
    {
        if (name.Length > Specifications.CacheEntryNameMaxLength)
            throw new FieldTooLongException(nameof(Name), Specifications.CacheEntryNameMaxLength, name.Length);
        
        if (name.Length < Specifications.CacheEntryNameMinLength)
            throw new FieldTooShortException(nameof(Name), Specifications.CacheEntryNameMinLength, name.Length);

        Name = name;
    }
    
    public string Name { get; }
}