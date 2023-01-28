namespace Osekai.Octon.Persistence.Repositories;

public interface IPermissionChecker
{
    Task<bool> HasPermission(int userId, string permission, CancellationToken cancellationToken = default);
}