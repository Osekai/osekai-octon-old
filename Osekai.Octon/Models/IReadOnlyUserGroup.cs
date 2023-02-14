using System.Drawing;
using Osekai.Octon.Enums;

namespace Osekai.Octon.Models;

public interface IReadOnlyUserGroup
{
    int Id { get; }
    string Name { get; }
    string ShortName { get; } 
    string Description { get; }
    Color Colour { get;  }
    int Order { get; }
    bool Hidden { get; }
    bool ForceVisibleInComments { get;}
    IReadOnlyDictionary<string, PermissionActionType> Permissions { get; }
}