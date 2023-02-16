namespace Osekai.Octon.Query;

public interface IParameterizedQuery<T, in TParam>
{
    Task<T> ExecuteAsync(TParam param, CancellationToken cancellationToken);
}