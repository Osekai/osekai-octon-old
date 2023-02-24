using Osekai.Octon.Permissions;

namespace Osekai.Octon.Persistence.EntityFramework.MySql.Entities;

internal sealed class UserGroup
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

    public Domain.AggregateRoots.UserGroup ToAggregateRoot()
    {
        return new Domain.AggregateRoots.UserGroup(
            Id, Name, ShortName, Description, 
            ColorFormatConversion.GetColorFromString(Colour),
            Order, Hidden, ForceVisibleInComments, Permissions);
    }
}