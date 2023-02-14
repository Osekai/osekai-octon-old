namespace Osekai.Octon.RichModels;

public interface ISavableEntity
{
    Task PublishChangesAsync(CancellationToken cancellationToken = default);
}