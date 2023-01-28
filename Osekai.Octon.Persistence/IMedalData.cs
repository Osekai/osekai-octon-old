namespace Osekai.Octon.Persistence;

public interface IDataAggregator<T>
{
    Task<T> AggregateAsync(CancellationToken cancellationToken);
}