using Osekai.Octon.Domain.AggregateRoots;

namespace Osekai.Octon.Domain.Services;

public interface IUserGroupService
{
    Task<IEnumerable<UserGroup>> GetUserGroupsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<UserGroup>> GetUserGroupsOfUserAsync(int userId, CancellationToken cancellationToken = default);
}