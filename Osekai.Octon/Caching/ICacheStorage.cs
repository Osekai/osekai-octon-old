namespace Osekai.Octon.Caching;

public interface ICacheStorage
{
    Task PutEntryAsync(string name, ReadOnlyMemory<byte> data, long expiresAfter, CancellationToken cancellationToken = default);
    Task<ReadOnlyMemory<byte>?> GetEntryAsync(string name, CancellationToken cancellationToken = default);
    Task DeleteEntryAsync(string name, CancellationToken cancellationToken = default);
}