using MessagePack;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IO;

namespace Osekai.Octon.Caching.MsgPack;

public abstract class MsgPackCache: ICache
{
    public class MsgPackContainer<T>
    {
        public MsgPackContainer(T? content)
        {
            Content = content;
        }

        public T? Content { get; }
    }
    
    protected IServiceScopeFactory ServiceScopeFactory { get; }
    protected RecyclableMemoryStreamManager RecyclableMemoryStreamManager { get; }
    
    public MsgPackCache(
        IServiceScopeFactory serviceScopeFactory,
        RecyclableMemoryStreamManager recyclableMemoryStreamManager)
    {
        ServiceScopeFactory = serviceScopeFactory;
        RecyclableMemoryStreamManager = recyclableMemoryStreamManager;
    }

    protected abstract Task<ReadOnlyMemory<byte>?> GetEntryDataAsync(string name, CancellationToken cancellationToken = default);
    protected abstract Task SaveEntryDataAsync(string name, ReadOnlyMemory<byte> data, long expiresAfter, CancellationToken cancellationToken = default);
    protected abstract Task DeleteEntryAsync(string name, CancellationToken cancellationToken = default);
    
    public async Task<T?> GetAsync<T>(string name, CancellationToken cancellationToken = default) where T: class
    {
        ReadOnlyMemory<byte>? data = await GetEntryDataAsync(name, cancellationToken);

        if (data == null)
            return null;

        return MessagePackSerializer.Deserialize<MsgPackContainer<T>>(data.Value,
            MessagePack.Resolvers.ContractlessStandardResolver.Options).Content;
    }

    public async Task SetAsync<T>(string name, T? data, long expiresAfter = 3600, CancellationToken cancellationToken = default) where T: class
    {
        using MemoryStream memoryStream = RecyclableMemoryStreamManager.GetStream();

        await MessagePackSerializer.SerializeAsync(memoryStream, new MsgPackContainer<T>(data),
            MessagePack.Resolvers.ContractlessStandardResolver.Options, cancellationToken);

        if (memoryStream.Length > int.MaxValue)
            throw new ArgumentException("Data too big");
        
        await SaveEntryDataAsync(name, memoryStream.GetBuffer()[..(int)memoryStream.Length], expiresAfter, cancellationToken);
    }

    public Task DeleteAsync(string name, CancellationToken cancellationToken = default) =>
        DeleteEntryAsync(name, cancellationToken);
}