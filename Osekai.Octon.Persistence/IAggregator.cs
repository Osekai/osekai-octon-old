namespace Osekai.Octon.Persistence;

public interface IAggregator<T>
{
    Task<IEnumerable<T>> AggregateAllAsync(CancellationToken cancellationToken);
}