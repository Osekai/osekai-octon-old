using System.Collections.Immutable;
using Osekai.Octon.Database.HelperTypes;

namespace Osekai.Octon.Database.Dtos;

public class UserPermissionsOverrideDto
{
    public UserPermissionsOverrideDto(int userId, IReadOnlyDictionary<string, PermissionActionType> permissionActionType)
    {
        UserId = userId;
        Permissions = new Dictionary<string, PermissionActionType>(permissionActionType);
    }
    
    public UserPermissionsOverrideDto(int userId, IDictionary<string, PermissionActionType> permissionActionType)
    {
        UserId = userId;
        Permissions = new Dictionary<string, PermissionActionType>(permissionActionType);
    }
    
    public int UserId { get; }
    public IReadOnlyDictionary<string, PermissionActionType> Permissions { get; } 
}