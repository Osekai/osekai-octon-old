using Osekai.Octon.Database.HelperTypes;

namespace Osekai.Octon.Permissions;

public static class PermissionUtils
{
    public static IEnumerable<string> MergePermissionDictionaries(IEnumerable<IReadOnlyDictionary<string, PermissionActionType>> permissionDictionaries)
    {
        Dictionary<string, PermissionActionType> rDictionary = new Dictionary<string, PermissionActionType>();

        foreach (var permissionDictionary in permissionDictionaries)
        foreach (KeyValuePair<string, PermissionActionType> permission in permissionDictionary)
            rDictionary.TryAdd(permission.Key, permission.Value);

        return rDictionary.Where(kv => kv.Value != PermissionActionType.Deny).Select(kv => kv.Key);
    }
}