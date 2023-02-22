using Osekai.Octon.Domain.Aggregates;
using Osekai.Octon.Domain.Entities;

namespace Osekai.Octon.Domain.Repositories;

public interface IUserGroupRepository
{
    Task<UserGroup?> GetUserGroupByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserGroup>> GetUserGroupsOfUserAsync(int userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserGroup>> GetUserGroups(CancellationToken cancellationToken = default);
}