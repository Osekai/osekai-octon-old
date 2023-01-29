using System.Collections.Immutable;
using System.Drawing;
using Osekai.Octon.Enums;
using Osekai.Octon.Persistence;

namespace Osekai.Octon.Services.Entities;

public class UserGroup
{
    protected internal IUnitOfWork UnitOfWork { get; }
    
    protected internal UserGroup(int id, string name, string shortName, IUnitOfWork unitOfWork, IReadOnlyDictionary<string, PermissionActionType>? permissions = null, 
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
        Permissions = permissions != null ? new Dictionary<string, PermissionActionType>(permissions) : ImmutableDictionary<string, PermissionActionType>.Empty;
        UnitOfWork = unitOfWork;
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