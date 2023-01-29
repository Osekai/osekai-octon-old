using MessagePack;
using Microsoft.Extensions.DependencyInjection;
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
    
    protected IServiceScopeFactory ServiceScopeFactory { get; }
    protected RecyclableMemoryStreamManager RecyclableMemoryStreamManager { get; }
    
    public MsgPackDatabaseCache(
        IServiceScopeFactory serviceScopeFactory,
        RecyclableMemoryStreamManager recyclableMemoryStreamManager)
    {
        ServiceScopeFactory = serviceScopeFactory;
        RecyclableMemoryStreamManager = recyclableMemoryStreamManager;
    }

    
    public async Task<T?> GetAsync<T>(string name, CancellationToken cancellationToken = default) where T: class
    {
        await using AsyncServiceScope scope = ServiceScopeFactory.CreateAsyncScope();
        IUnitOfWork unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        
        CacheEntryDto? cacheEntry = await unitOfWork.CacheEntryRepository.GetCacheEntryByNameAsync(name, cancellationToken);

        if (cacheEntry == null)
            return null;

        return MessagePackSerializer.Deserialize<MsgPackContainer<T>>(cacheEntry.Data,
            MessagePack.Resolvers.ContractlessStandardResolver.Options).Content;
    }

    public async Task SetAsync<T>(string name, T? data, long expiresAfter = 3600, CancellationToken cancellationToken = default) where T: class
    {
        await using AsyncServiceScope scope = ServiceScopeFactory.CreateAsyncScope();
        IUnitOfWork unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        
        using MemoryStream memoryStream = RecyclableMemoryStreamManager.GetStream();

        await MessagePackSerializer.SerializeAsync(memoryStream, new MsgPackContainer<T>(data),
            MessagePack.Resolvers.ContractlessStandardResolver.Options, cancellationToken);
        
        await unitOfWork.CacheEntryRepository.AddOrUpdateCacheEntryAsync(
            name, memoryStream.GetBuffer()[0..(int)memoryStream.Length], DateTimeOffset.Now.AddSeconds(expiresAfter),
            cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }


    public async Task DeleteAsync(string name, CancellationToken cancellationToken = default)
    {
        await using AsyncServiceScope scope = ServiceScopeFactory.CreateAsyncScope();
        IUnitOfWork unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        
        await unitOfWork.CacheEntryRepository.DeleteCacheEntryAsync(name, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}