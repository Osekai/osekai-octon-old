namespace Osekai.Octon;

public interface IAdapter<in TIn, TOut>
{
    Task<TOut> AdaptAsync(TIn value, CancellationToken cancellationToken = default);
}