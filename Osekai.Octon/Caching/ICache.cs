namespace Osekai.Octon.Caching;

public interface ICache
{
    Task<object?> GetAsync(Type type, string name, CancellationToken cancellationToken = default);
    Task SetAsync(string name, object data, long expiresAfter = 3600, CancellationToken cancellationToken = default);
    Task DeleteAsync(string name, CancellationToken cancellationToken = default);
}