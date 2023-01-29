using MessagePack;
using Microsoft.IO;
using Osekai.Octon.Persistence;
using Osekai.Octon.Persistence.Dtos;

namespace Osekai.Octon.Caching.MsgPack;

public class MsgPackDatabaseCache: ICache
{
    public class MsgPackContainer<T>
    {
        public MsgPackContainer(T? content)
        {
            Content = content;
        }

        public T? Content { get; }
    }
    
    protected IUnitOfWork UnitOfWork { get; }
    protected RecyclableMemoryStreamManager RecyclableMemoryStreamManager { get; }
    
    public MsgPackDatabaseCache(
        IUnitOfWork unitOfWork,
        RecyclableMemoryStreamManager recyclableMemoryStreamManager)
    {
        UnitOfWork = unitOfWork;
        RecyclableMemoryStreamManager = recyclableMemoryStreamManager;
    }

    
    public async Task<T?> GetAsync<T>(string name, CancellationToken cancellationToken = default) where T: class
    {
        CacheEntryDto? cacheEntry = await UnitOfWork.CacheEntryRepository.GetCacheEntryByNameAsync(name, cancellationToken);

        if (cacheEntry == null)
            return null;

        return MessagePackSerializer.Deserialize<MsgPackContainer<T>>(cacheEntry.Data,
            MessagePack.Resolvers.ContractlessStandardResolver.Options).Content;
    }

    public async Task SetAsync<T>(string name, T? data, long expiresAfter = 3600, CancellationToken cancellationToken = default) where T: class
    {
        using MemoryStream memoryStream = RecyclableMemoryStreamManager.GetStream();

        await MessagePackSerializer.SerializeAsync(memoryStream, new MsgPackContainer<T>(data),
            MessagePack.Resolvers.ContractlessStandardResolver.Options, cancellationToken);
        
        await UnitOfWork.CacheEntryRepository.AddOrUpdateCacheEntryAsync(
            name, memoryStream.GetBuffer()[0..(int)memoryStream.Length], DateTimeOffset.Now.AddSeconds(expiresAfter),
            cancellationToken);
    }


    public async Task DeleteAsync(string name, CancellationToken cancellationToken = default)
    {
        await UnitOfWork.CacheEntryRepository.DeleteCacheEntryAsync(name, cancellationToken);
    }
}