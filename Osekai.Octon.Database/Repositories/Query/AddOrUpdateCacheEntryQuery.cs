using Osekai.Octon.Exceptions;

namespace Osekai.Octon.Database.Repositories.Query;

public readonly struct AddOrUpdateCacheEntryQuery
{
    public AddOrUpdateCacheEntryQuery(string name, byte[] data, long expiresAfter)
    {
        if (name.Length > Specifications.CacheEntryNameMaxLength)
            throw new FieldTooLongException(nameof(Name), Specifications.CacheEntryNameMaxLength, name.Length);
        
        if (name.Length < Specifications.CacheEntryNameMinLength)
            throw new FieldTooShortException(nameof(Name), Specifications.CacheEntryNameMinLength, name.Length);

        if (data.Length > Specifications.CacheEntryDataMaxLength)
            throw new FieldTooLongException(nameof(Data), Specifications.CacheEntryDataMaxLength, data.Length);

        Name = name;
        Data = data;
        ExpiresAt = DateTime.Now.AddSeconds(expiresAfter);
    }
    
    public string Name { get; }
    public byte[] Data { get; }
    public DateTime ExpiresAt { get; }
}