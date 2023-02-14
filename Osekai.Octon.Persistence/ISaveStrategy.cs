namespace Osekai.Octon.Persistence;

public interface ISaveStrategy: ICloneable, IDisposable, IAsyncDisposable
{
    Task SaveAsync(CancellationToken cancellationToken = default);
}