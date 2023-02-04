using Osekai.Octon.Objects;

namespace Osekai.Octon.Persistence.Repositories;

public interface IUserGroupRepository
{
    Task<IReadOnlyUserGroup?> GetUserGroupByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<IReadOnlyUserGroup>> GetUserGroupsOfUserAsync(int userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<IReadOnlyUserGroup>> GetUserGroups(CancellationToken cancellationToken = default);
}