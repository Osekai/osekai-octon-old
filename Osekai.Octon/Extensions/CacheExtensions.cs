using Osekai.Octon.Caching;

namespace Osekai.Octon.Extensions;

public static class CacheExtensions
{
    public static async Task<T?> GetAsync<T>(this ICache cache, string name, CancellationToken cancellationToken = default) =>
        (T?)await cache.GetAsync(typeof(T), name, cancellationToken);

    public static Task SetAsync<T>(this ICache cache, string name, T value, long expiresAfter = 3600, CancellationToken cancellationToken = default) =>
        cache.SetAsync(name, value ?? throw new ArgumentNullException(nameof(value)), expiresAfter, cancellationToken);
}