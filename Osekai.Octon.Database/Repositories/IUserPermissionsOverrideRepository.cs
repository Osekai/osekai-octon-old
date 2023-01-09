using Osekai.Octon.Database.Dtos;

namespace Osekai.Octon.Database.Repositories;

public interface IUserPermissionsOverrideRepository
{
    Task<UserPermissionsOverrideDto?> GetUserPermissionOverrideByUserId(int userId, CancellationToken cancellationToken = default);
}