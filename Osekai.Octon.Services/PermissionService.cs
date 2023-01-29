using Osekai.Octon.Enums;
using Osekai.Octon.Permissions;
using Osekai.Octon.Persistence;
using Osekai.Octon.Persistence.Dtos;
using Osekai.Octon.Services.Entities;
using Osekai.Octon.Services.PermissionStores;

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
        IEnumerable<UserGroupDto> userGroups = await UnitOfWork.UserGroupRepository.GetUserGroupsOfUserAsync(userId, cancellationToken);
        UserPermissionsOverrideDto? permissionsOverrideDto = await UnitOfWork.UserPermissionsOverrideRepository.GetUserPermissionOverrideByUserId(userId, cancellationToken);

        IEnumerable<IReadOnlyDictionary<string, PermissionActionType>> permissionDictionaries = userGroups.Select(u => u.Permissions);

        if (permissionsOverrideDto != null)
            permissionDictionaries = permissionDictionaries.Concat(Enumerable.Repeat(permissionsOverrideDto.Permissions, 1));

        return new InMemoryPermissionStore(permissionDictionaries);
    }
}