using Osekai.Octon.Enums;

namespace Osekai.Octon.Services;

public interface IPermissionStore
{
    Task<bool> HasPermissionAsync(string path);
    Task<IEnumerable<KeyValuePair<string, PermissionActionType>>> GetPermissionsAsync();
}