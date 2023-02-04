using Osekai.Octon.Enums;
using Osekai.Octon.Objects;

namespace Osekai.Octon.Persistence.EntityFramework.MySql.Dtos;

internal sealed class UserPermissionsOverrideDto: IReadOnlyUserPermissionOverride
{
    public UserPermissionsOverrideDto(int userId, IDictionary<string, PermissionActionType> permissionActionType)
    {
        UserId = userId;
        Permissions = new SortedList<string, PermissionActionType>(permissionActionType);
    }
    
    public int UserId { get; }
    public IReadOnlyDictionary<string, PermissionActionType> Permissions { get; } 
}