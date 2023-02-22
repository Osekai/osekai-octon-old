using Microsoft.Extensions.Caching.Memory;

namespace Osekai.Octon.Caching.Storages.MicrosoftInMemoryCache;

public class MicrosoftInMemoryCacheStorage: ICacheStorage
{
    protected IMemoryCache MemoryCache { get; } 
    
    public MicrosoftInMemoryCacheStorage(IMemoryCache memoryCache)
    {
        MemoryCache = memoryCache;
    }
    
    public Task PutEntryAsync(string name, ReadOnlyMemory<byte> data, long expiresAfter, CancellationToken cancellationToken = default)
    {
        MemoryCache.Set(name, data, DateTimeOffset.Now.AddSeconds(expiresAfter));
        return Task.CompletedTask;
    }

    public Task<ReadOnlyMemory<byte>?> GetEntryAsync(string name, CancellationToken cancellationToken = default)
    {
        if (!MemoryCache.TryGetValue(name, out object? o))
            return Task.FromResult<ReadOnlyMemory<byte>?>(null);

        if (o is not ReadOnlyMemory<byte> data)
            throw new InvalidDataException();

        return Task.FromResult<ReadOnlyMemory<byte>?>(data);
    }

    public Task DeleteEntryAsync(string name, CancellationToken cancellationToken = default)
    {
        MemoryCache.Remove(name);
        return Task.CompletedTask;
    }
}