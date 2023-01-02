namespace Osekai.Octon.Database.Dtos;

public class CacheEntryDto
{
    public CacheEntryDto(string name, byte[] data, DateTimeOffset expiresAt)
    {
        Name = name;
        Data = data;
        ExpiresAt = expiresAt;
    }
    
    public string Name { get; } 
    public byte[] Data { get; } 
    public DateTimeOffset ExpiresAt { get; } 
}