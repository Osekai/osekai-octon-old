namespace Osekai.Octon;

public interface IParameterizedQuery<T, in TParam>
{
    Task<T> ExecuteAsync(TParam param, CancellationToken cancellationToken);
}