namespace Osekai.Octon.Persistence;

public interface IQuery<T>
{
    Task<IEnumerable<T>> ExecuteAsync(CancellationToken cancellationToken);
}