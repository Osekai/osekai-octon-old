using System.Collections.Immutable;
using System.Drawing;
using Osekai.Octon.Enums;
using Osekai.Octon.Models;
using Osekai.Octon.Persistence;

namespace Osekai.Octon.RichModels;

public class UserGroup: IReadOnlyUserGroup
{
    protected internal IUnitOfWork UnitOfWork { get; }
    
    protected internal UserGroup(int id, string name, string shortName, IReadOnlyDictionary<string, PermissionActionType> permissions, 
        string description, Color colour, int order, bool hidden, bool forceVisibleInComments, IUnitOfWork unitOfWork)
    {
        Id = id;
        Name = name;
        ShortName = shortName;
        Description = description;
        Colour = colour;
        Order = order;
        Hidden = hidden;
        ForceVisibleInComments = forceVisibleInComments;
        Permissions = new Dictionary<string, PermissionActionType>(permissions);
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