using Osekai.Octon.Enums;

namespace Osekai.Octon.Models;

public interface IReadOnlyUserPermissionOverride
{
    int UserId { get; }
    IReadOnlyDictionary<string, PermissionActionType> Permissions { get; } 
}