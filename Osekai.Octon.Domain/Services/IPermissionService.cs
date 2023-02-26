using Osekai.Octon.Permissions;

namespace Osekai.Octon.Domain.Services;

public interface IPermissionService
{
    Task<IPermissionStore> GetPermissionStoreAsync(int userId, CancellationToken cancellationToken = default);
}