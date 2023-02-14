using Osekai.Octon.Models;

namespace Osekai.Octon.Persistence.Repositories;

public interface IUserPermissionsOverrideRepository
{
    Task<IReadOnlyUserPermissionOverride?> GetUserPermissionOverrideByUserId(int userId, CancellationToken cancellationToken = default);
}