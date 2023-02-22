using Osekai.Octon.Domain.Aggregates;
using Osekai.Octon.Domain.Entities;

namespace Osekai.Octon.Domain.Repositories;

public interface IUserPermissionsOverrideRepository
{
    Task<UserPermissionsOverride?> GetUserPermissionOverrideByUserId(int userId, CancellationToken cancellationToken = default);
}