using Osekai.Octon.Permissions;

namespace Osekai.Octon.Domain.Aggregates;

public class UserPermissionsOverride
{
    public UserPermissionsOverride(int userId, IDictionary<string, PermissionActionType> permissions)
    {
        UserId = userId;
        Permissions = new SortedList<string, PermissionActionType>(permissions);
    }
    
    public int UserId { get; }
    public IReadOnlyDictionary<string, PermissionActionType> Permissions { get; } 
}