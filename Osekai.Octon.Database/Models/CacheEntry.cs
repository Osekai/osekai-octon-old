namespace Osekai.Octon.Database.Models;

public class CacheEntry
{
    public string Name { get; set; } = null!;
    public byte[] Data { get; set; } = null!;
    public DateTimeOffset ExpiresAt { get; set; } 
}