using Osekai.Octon.Domain.AggregateRoots;
using Osekai.Octon.Domain.AggregateRoots;

namespace Osekai.Octon.Domain.Repositories;

public interface IUserPermissionsOverrideRepository
{
    Task<UserPermissionsOverride?> GetUserPermissionOverrideByUserId(int userId, CancellationToken cancellationToken = default);
}