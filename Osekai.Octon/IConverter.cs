namespace Osekai.Octon;

public interface IConverter<in TIn, TOut>
{
    ValueTask<TOut> AdaptAsync(TIn value, CancellationToken cancellationToken = default);
}