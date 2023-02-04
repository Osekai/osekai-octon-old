using System.Collections.Immutable;
using System.Drawing;
using Osekai.Octon.Enums;
using Osekai.Octon.Objects;

namespace Osekai.Octon.Persistence.EntityFramework.MySql.Dtos;

internal sealed class UserGroupDto: IReadOnlyUserGroup
{
    public UserGroupDto(int id, string name, string shortName, IDictionary<string, PermissionActionType>? permissions = null, 
        string description = "", Color? colour = null, int order = 0, bool hidden = false, bool forceVisibleInComments = false)
    {
        Id = id;
        Name = name;
        ShortName = shortName;
        Description = description;
        Colour = colour ?? Color.White;
        Order = order;
        Hidden = hidden;
        ForceVisibleInComments = forceVisibleInComments;
        Permissions = permissions != null ? new SortedList<string, PermissionActionType>(permissions) : ImmutableDictionary<string, PermissionActionType>.Empty;
    }
    
    public int Id { get; }
    public string Name { get; }
    public string ShortName { get; } 
    public string Description { get; }
    public Color Colour { get;  }
    public int Order { get; }
    public bool Hidden { get; }
    public bool ForceVisibleInComments { get;}
    public IReadOnlyDictionary<string, PermissionActionType> Permissions { get; }
}