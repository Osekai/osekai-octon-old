namespace Osekai.Octon;

public interface IConverter<in TIn, TOut>
{
    ValueTask<TOut> ConvertAsync(TIn value, CancellationToken cancellationToken = default);
}