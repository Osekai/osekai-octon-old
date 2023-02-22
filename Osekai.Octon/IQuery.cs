namespace Osekai.Octon;

public interface IQuery<T>
{
    Task<T> ExecuteAsync(CancellationToken cancellationToken);
}