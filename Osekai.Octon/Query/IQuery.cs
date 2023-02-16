namespace Osekai.Octon.Query;

public interface IQuery<T>
{
    Task<T> ExecuteAsync(CancellationToken cancellationToken);
}