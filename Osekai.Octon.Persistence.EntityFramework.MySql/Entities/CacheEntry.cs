namespace Osekai.Octon.Persistence.EntityFramework.MySql.Entities;

internal sealed class CacheEntry
{
    public string Name { get; set; } = null!;
    public byte[] Data { get; set; } = null!;
    public DateTimeOffset ExpiresAt { get; set; }
}