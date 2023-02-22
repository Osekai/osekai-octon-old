using Osekai.Octon.Domain.Aggregates;
using Osekai.Octon.Domain.Entities;
using Osekai.Octon.Permissions;
using Osekai.Octon.Permissions.PermissionStores;
using Osekai.Octon.Persistence;

namespace Osekai.Octon.Services;

public class PermissionService
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