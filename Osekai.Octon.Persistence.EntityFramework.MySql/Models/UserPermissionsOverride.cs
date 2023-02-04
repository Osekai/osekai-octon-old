using Osekai.Octon.Enums;
using Osekai.Octon.Persistence.EntityFramework.MySql.Dtos;

namespace Osekai.Octon.Persistence.EntityFramework.MySql.Models;

internal sealed class UserPermissionsOverride
{
    public UserPermissionsOverride()
    {
        Permissions = new Dictionary<string, PermissionActionType>();
    }
    
    public int Id { get; set; }
    public int UserId { get; set; }
    public IDictionary<string, PermissionActionType> Permissions { get; set; }

    public UserPermissionsOverrideDto ToDto()
    {
        return new UserPermissionsOverrideDto(UserId, Permissions);
    }
}