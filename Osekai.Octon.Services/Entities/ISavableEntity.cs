namespace Osekai.Octon.Services.Entities;

public interface ISavableEntity
{
    Task SaveAsync(CancellationToken cancellationToken = default);
}