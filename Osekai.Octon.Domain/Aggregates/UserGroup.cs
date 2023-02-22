using System.Drawing;
using Osekai.Octon.Drawing;
using Osekai.Octon.Permissions;

namespace Osekai.Octon.Domain.Aggregates;

public class UserGroup
{
    public UserGroup(int id, string name, string shortName, string description,
        RgbColour colour, int order, bool hidden, bool forceVisibleInComments, IReadOnlyDictionary<string, PermissionActionType> permissions)
    {
        Id = id;
        Name = name;
        ShortName = shortName;
        Description = description;
        Colour = colour;
        Order = order;
        Hidden = hidden;
        ForceVisibleInComments = forceVisibleInComments;
        
        _permissions = new Dictionary<string, PermissionActionType>(permissions);
    }
    
    public UserGroup(int id, string name, string shortName, string description,
        RgbColour colour, int order, bool hidden, bool forceVisibleInComments, IDictionary<string, PermissionActionType> permissions)
    {
        Id = id;
        Name = name;
        ShortName = shortName;
        Description = description;
        Colour = colour;
        Order = order;
        Hidden = hidden;
        ForceVisibleInComments = forceVisibleInComments;
        
        _permissions = new Dictionary<string, PermissionActionType>(permissions);
    }

    private string _name = null!;
    private string _shortName = null!;
    private string _description = null!;
    private IReadOnlyDictionary<string, PermissionActionType> _permissions;

    public int Id { get; }

    public string Name
    {
        get => _name;
        set
        {
            if (value.Length is < Specifications.GroupNameMinLength or > Specifications.GroupNameMaxLength)
                throw new ArgumentOutOfRangeException(nameof(value), value.Length, $"Invalid {nameof(Name)} length");

            _name = value;
        }
    }

    public string ShortName
    {
        get => _shortName;
        set
        {
            if (value.Length is < Specifications.GroupShortNameMinLength or > Specifications.GroupShortNameMaxLength)
                throw new ArgumentOutOfRangeException(nameof(value), value.Length, $"Invalid {nameof(ShortName)} length");

            _shortName = value;
        }
    }

    public string Description
    {
        get => _description;
        set
        {
            if (value.Length is < Specifications.GroupDescriptionMinLength or > Specifications.GroupDescriptionMaxLength)
                throw new ArgumentOutOfRangeException(nameof(value), value.Length, $"Invalid {nameof(Description)} length");

            _description = value;
        }
    }
    public RgbColour Colour { get; set; }
    public int Order { get; set; }
    public bool Hidden { get; set; }
    public bool ForceVisibleInComments { get; set; }
    
    public IReadOnlyDictionary<string, PermissionActionType> Permissions
    {
        get => _permissions;
        set => _permissions = new Dictionary<string, PermissionActionType>(value);
    }
}