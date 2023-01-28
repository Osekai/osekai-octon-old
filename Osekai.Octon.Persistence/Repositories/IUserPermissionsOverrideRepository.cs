using Osekai.Octon.Persistence.Dtos;

namespace Osekai.Octon.Persistence.Repositories;

public interface IUserPermissionsOverrideRepository
{
    Task<UserPermissionsOverrideDto?> GetUserPermissionOverrideByUserId(int userId, CancellationToken cancellationToken = default);
}