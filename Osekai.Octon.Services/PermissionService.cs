using Osekai.Octon.Database;
using Osekai.Octon.Database.Dtos;
using Osekai.Octon.Database.HelperTypes;
using Osekai.Octon.Permissions;

namespace Osekai.Octon.Services;

public class PermissionService
{
    private IDatabaseUnitOfWorkFactory _databaseUnitOfWorkFactory;
    
    public PermissionService(IDatabaseUnitOfWorkFactory databaseUnitOfWorkFactory)
    {
        _databaseUnitOfWorkFactory = databaseUnitOfWorkFactory;
    }

    public async Task<IPermissionCollection> GetUserPermissionsAsync(int userId, CancellationToken cancellationToken = default)
    { 
        await using IDatabaseUnitOfWork unitOfWork = await _databaseUnitOfWorkFactory.CreateAsync();
        
        IEnumerable<UserGroupDto> userGroups = await unitOfWork.UserGroupRepository.GetUserGroupOfUserAsync(userId, cancellationToken);
        UserPermissionsOverrideDto? permissionsOverrideDto = await unitOfWork.UserPermissionsOverrideRepository.GetUserPermissionOverrideByUserId(userId, cancellationToken);

        IEnumerable<IReadOnlyDictionary<string, PermissionActionType>> permissionDictionaries = userGroups.Select(u => u.Permissions);

        if (permissionsOverrideDto != null)
            permissionDictionaries = permissionDictionaries.Concat(Enumerable.Repeat(permissionsOverrideDto.Permissions, 1));

        return new InMemoryPermissionCollection(PermissionUtils.MergePermissionDictionaries(permissionDictionaries.Reverse()));
    }
}