using Osekai.Octon.Persistence;
using UserGroup = Osekai.Octon.Domain.AggregateRoots.UserGroup;

namespace Osekai.Octon.Services;

public class UserGroupService
{
    protected IUnitOfWork UnitOfWork { get; }
    
    public UserGroupService(IUnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork;
    }

    public Task<IEnumerable<UserGroup>> GetUserGroupsAsync(CancellationToken cancellationToken = default)
        => UnitOfWork.UserGroupRepository.GetUserGroups(cancellationToken);

    public Task<IEnumerable<UserGroup>> GetUserGroupsOfUserAsync(int userId, CancellationToken cancellationToken = default)
        => UnitOfWork.UserGroupRepository.GetUserGroupsOfUserAsync(userId, cancellationToken);
}