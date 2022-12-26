using MessagePack;
using Microsoft.IO;
using Osekai.Octon.Database;
using Osekai.Octon.Database.Models;
using Osekai.Octon.Database.Repositories.Query;
using Osekai.Octon.Services;

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
    
    private readonly IDatabaseUnitOfWork _databaseUnitOfWork;
    private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;
    
    public MsgPackDatabaseCache(
        IDatabaseUnitOfWork databaseUnitOfWork,
        RecyclableMemoryStreamManager recyclableMemoryStreamManager)
    {
        _databaseUnitOfWork = databaseUnitOfWork;
        _recyclableMemoryStreamManager = recyclableMemoryStreamManager;
    }

    
    public async Task<T?> GetAsync<T>(string name, CancellationToken cancellationToken = default) where T: class
    {
        CacheEntry? cacheEntry = await _databaseUnitOfWork.CacheEntryRepository.GetCacheEntryFromNameAsync(
            new GetCacheEntryFromNameQuery(name),
            cancellationToken);

        if (cacheEntry == null)
            return null;

        return MessagePackSerializer.Deserialize<MsgPackContainer<T>>(cacheEntry.Data,
            MessagePack.Resolvers.ContractlessStandardResolver.Options).Content;
    }

    public async Task SetAsync<T>(string name, T? data, long expiresAfter = 3600, CancellationToken cancellationToken = default) where T: class
    {
        using MemoryStream memoryStream = _recyclableMemoryStreamManager.GetStream();
        
        await MessagePackSerializer.SerializeAsync(memoryStream, new MsgPackContainer<T>(data),
            MessagePack.Resolvers.ContractlessStandardResolver.Options, cancellationToken);
        
        await _databaseUnitOfWork.CacheEntryRepository.AddOrUpdateCacheEntryAsync(
            new AddOrUpdateCacheEntryQuery(name, memoryStream.GetBuffer()[0..(int)memoryStream.Length], expiresAfter),
            cancellationToken);

        await _databaseUnitOfWork.SaveAsync(cancellationToken);
    }


    public async Task DeleteAsync(string name, CancellationToken cancellationToken = default)
    {
        await _databaseUnitOfWork.CacheEntryRepository.DeleteCacheEntryAsync(
            new DeleteCacheEntryQuery(name),
            cancellationToken);

        await _databaseUnitOfWork.SaveAsync(cancellationToken);
    }
}