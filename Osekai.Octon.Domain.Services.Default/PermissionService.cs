using Osekai.Octon.Domain.AggregateRoots;
using Osekai.Octon.Domain.Services;
using Osekai.Octon.Permissions;
using Osekai.Octon.Permissions.PermissionStores;
using Osekai.Octon.Persistence;

namespace Osekai.Octon.Domain.Services.Default;

public class PermissionService : IPermissionService
{
    protected IUnitOfWork UnitOfWork { get; }
    
    public PermissionService(IUnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork;
    }
    
    public async Task<IPermissionStore> GetPermissionStoreAsync(int userId, CancellationToken cancellationToken = default)
    {
        IEnumerable<UserGroup> userGroups = await UnitOfWork.UserGroupRepository.GetUserGroupsOfUserAsync(userId, cancellationToken);
        UserPermissionsOverride? permissionsOverrideDto = await UnitOfWork.UserPermissionsOverrideRepository.GetUserPermissionOverrideByUserId(userId, cancellationToken);

        IEnumerable<IReadOnlyDictionary<string, PermissionActionType>> permissionDictionaries = userGroups.Select(u => u.Permissions);

        if (permissionsOverrideDto != null)
            permissionDictionaries = permissionDictionaries.Concat(Enumerable.Repeat(permissionsOverrideDto.Permissions, 1));

        return new InMemoryPermissionStore(permissionDictionaries);
    }
}