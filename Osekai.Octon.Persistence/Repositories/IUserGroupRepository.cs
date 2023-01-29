using Osekai.Octon.Persistence.Dtos;

namespace Osekai.Octon.Persistence.Repositories;

public interface IUserGroupRepository
{
    Task<UserGroupDto?> GetUserGroupByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserGroupDto>> GetUserGroupsOfUserAsync(int userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserGroupDto>> GetUserGroups(CancellationToken cancellationToken = default);
}