namespace Osekai.Octon;

public interface ICache
{
    Task<T?> GetAsync<T>(string name, CancellationToken cancellationToken = default) where T: class;
    Task SetAsync<T>(string name, T data, long expiresAfter = 3600, CancellationToken cancellationToken = default) where T: class?;
    Task DeleteAsync(string name, CancellationToken cancellationToken = default);
}