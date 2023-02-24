using Osekai.Octon.Domain.AggregateRoots;

namespace Osekai.Octon.Domain.Repositories;

public interface IUserGroupRepository
{
    Task<UserGroup?> GetUserGroupByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserGroup>> GetUserGroupsOfUserAsync(int userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserGroup>> GetUserGroups(CancellationToken cancellationToken = default);
}