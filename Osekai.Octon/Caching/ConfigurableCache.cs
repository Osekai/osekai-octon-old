using Microsoft.IO;

namespace Osekai.Octon.Caching;

public class ConfigurableCache: ICache
{
    protected ICacheCodec Codec { get; }
    protected ICacheStorage Storage { get; }
    protected RecyclableMemoryStreamManager RecyclableMemoryStreamManager { get; }
    
    public ConfigurableCache(RecyclableMemoryStreamManager recyclableMemoryStreamManager, ICacheStorage storage, ICacheCodec codec)
    {
        Codec = codec;
        Storage = storage;
        RecyclableMemoryStreamManager = recyclableMemoryStreamManager;
    }

    public async Task<object?> GetAsync(Type type, string name, CancellationToken cancellationToken = default)
    {
        ReadOnlyMemory<byte>? data = await Storage.GetEntryAsync(name, cancellationToken);
        if (data == null)
            return null;
        
        return Codec.DecodeObject(type, data.Value);
    }

    public Task SetAsync(string name, object data, long expiresAfter = 3600, CancellationToken cancellationToken = default)
    {
        using MemoryStream memoryStream = RecyclableMemoryStreamManager.GetStream();
        Codec.EncodeObject(data.GetType(), data, memoryStream);
        return Storage.PutEntryAsync(name, memoryStream.GetBuffer()[..(int)memoryStream.Length], expiresAfter, cancellationToken);
    }

    public Task DeleteAsync(string name, CancellationToken cancellationToken = default)
    {
        return Storage.DeleteEntryAsync(name, cancellationToken);
    }
}