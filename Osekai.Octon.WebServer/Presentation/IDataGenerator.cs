namespace Osekai.Octon.WebServer.Presentation;

public interface IDataGenerator<T>
{
    Task<T> GenerateAsync(CancellationToken cancellationToken = default);
}