namespace Osekai.Octon.Permissions;

public interface IPermissionStore
{
    Task<bool> HasPermissionAsync(string path);
    Task<IEnumerable<KeyValuePair<string, PermissionActionType>>> GetPermissionsAsync();
}