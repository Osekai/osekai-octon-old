using Osekai.Octon.Database.Dtos;

namespace Osekai.Octon.Database.EntityFramework.MySql.Models;

internal sealed class CacheEntry
{
    public string Name { get; set; } = null!;
    public byte[] Data { get; set; } = null!;
    public DateTimeOffset ExpiresAt { get; set; }

    public CacheEntryDto ToDto()
    {
        return new CacheEntryDto(Name, Data, ExpiresAt);
    }
}