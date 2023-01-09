using Osekai.Octon.Database.Dtos;

namespace Osekai.Octon.Database.Repositories;

public interface IUserGroupRepository
{
    Task<UserGroupDto?> GetUserGroupByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserGroupDto>> GetUserGroupOfUserAsync(int userId, CancellationToken cancellationToken = default);
}