using Osekai.Octon.Enums;

namespace Osekai.Octon.Objects;

public interface IReadOnlyUserPermissionOverride
{
    int UserId { get; }
    IReadOnlyDictionary<string, PermissionActionType> Permissions { get; } 
}