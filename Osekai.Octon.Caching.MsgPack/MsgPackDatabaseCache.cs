using MessagePack;
using Microsoft.IO;
using Osekai.Octon.Database;
using Osekai.Octon.Database.Dtos;

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
    
    private readonly IDatabaseUnitOfWorkFactory _databaseUnitOfWorkFactory;
    private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;
    
    public MsgPackDatabaseCache(
        IDatabaseUnitOfWorkFactory databaseUnitOfWorkFactory,
        RecyclableMemoryStreamManager recyclableMemoryStreamManager)
    {
        _databaseUnitOfWorkFactory = databaseUnitOfWorkFactory;
        _recyclableMemoryStreamManager = recyclableMemoryStreamManager;
    }

    
    public async Task<T?> GetAsync<T>(string name, CancellationToken cancellationToken = default) where T: class
    {
        IDatabaseUnitOfWork unitOfWork = await _databaseUnitOfWorkFactory.CreateAsync();
        CacheEntryDto? cacheEntry = await unitOfWork.CacheEntryRepository.GetCacheEntryByNameAsync(name, cancellationToken);

        if (cacheEntry == null)
            return null;

        return MessagePackSerializer.Deserialize<MsgPackContainer<T>>(cacheEntry.Data,
            MessagePack.Resolvers.ContractlessStandardResolver.Options).Content;
    }

    public async Task SetAsync<T>(string name, T? data, long expiresAfter = 3600, CancellationToken cancellationToken = default) where T: class
    {
        IDatabaseUnitOfWork unitOfWork = await _databaseUnitOfWorkFactory.CreateAsync();
        using MemoryStream memoryStream = _recyclableMemoryStreamManager.GetStream();
        
        await MessagePackSerializer.SerializeAsync(memoryStream, new MsgPackContainer<T>(data),
            MessagePack.Resolvers.ContractlessStandardResolver.Options, cancellationToken);
        
        await unitOfWork.CacheEntryRepository.AddOrUpdateCacheEntryAsync(
            name, memoryStream.GetBuffer()[0..(int)memoryStream.Length], DateTimeOffset.Now.AddSeconds(expiresAfter),
            cancellationToken);

        await unitOfWork.SaveAsync(cancellationToken);
    }


    public async Task DeleteAsync(string name, CancellationToken cancellationToken = default)
    {
        IDatabaseUnitOfWork unitOfWork = await _databaseUnitOfWorkFactory.CreateAsync();

        await unitOfWork.CacheEntryRepository.DeleteCacheEntryAsync(name, cancellationToken);
        await unitOfWork.SaveAsync(cancellationToken);
    }
}