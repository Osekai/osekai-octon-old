namespace Osekai.Octon;

public interface IAdapter<in TIn, TOut>
{
    ValueTask<TOut> AdaptAsync(TIn value, CancellationToken cancellationToken = default);
}