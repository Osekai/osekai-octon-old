using Osekai.Octon.Enums;
using Osekai.Octon.Persistence.Dtos;

namespace Osekai.Octon.Persistence.EntityFramework.MySql.Models;

public class UserGroup
{
    public UserGroup()
    {
        Permissions = new Dictionary<string, PermissionActionType>();
        UserGroupForUsers = new List<UserGroupsForUsers>();
    }
    
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string ShortName { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Colour { get; set; } = null!;
    public int Order { get; set; }
    public bool Hidden { get; set; }
    public bool ForceVisibleInComments { get; set; }
    public IDictionary<string, PermissionActionType> Permissions { get; set; }

    public ICollection<UserGroupsForUsers> UserGroupForUsers { get; set; }
    
    public UserGroupDto ToDto()
    {
        return new UserGroupDto(
            Id, Name, ShortName, Permissions, Description, 
            ColorFormatConversion.GetColorFromString(Colour),
            Order, Hidden, ForceVisibleInComments);
    }
}