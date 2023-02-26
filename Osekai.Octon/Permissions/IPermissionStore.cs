namespace Osekai.Octon.Permissions;

public interface IPermissionStore
{
    ValueTask<bool> HasPermissionAsync(string path);
    ValueTask<IEnumerable<KeyValuePair<string, PermissionActionType>>> GetPermissionsAsync();
}