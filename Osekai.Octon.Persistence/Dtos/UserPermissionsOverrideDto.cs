using Osekai.Octon.Enums;

namespace Osekai.Octon.Persistence.Dtos;

public class UserPermissionsOverrideDto
{
    public UserPermissionsOverrideDto(int userId, IDictionary<string, PermissionActionType> permissionActionType)
    {
        UserId = userId;
        Permissions = new SortedList<string, PermissionActionType>(permissionActionType);
    }
    
    public int UserId { get; }
    public IReadOnlyDictionary<string, PermissionActionType> Permissions { get; } 
}