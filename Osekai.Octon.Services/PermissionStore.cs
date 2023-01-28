
using Osekai.Octon.DataStructures;
using Osekai.Octon.Enums;
using Osekai.Octon.Permissions;

namespace Osekai.Octon.Services.Entities;

public class PermissionStore
{
    public bool HasPermission(string path)
    {
        ValueTrie<PermissionActionType>? node = Root.GetNodeRecursive(path);

        if (node == null)
            return false;

        if (path.EndsWith('*'))
        {
            if (!node.Wildcard.HasValue)
                return false;
            
            return node.Wildcard.Value.Value == PermissionActionType.Grant;
        }

        return node.Value == PermissionActionType.Grant;
    }

    private void AddPermission(string path, PermissionActionType type)
    {
        Root.AddValueRecursive(path, type);
    }

    public IEnumerable<KeyValuePair<string, PermissionActionType>> GetPermissions()
        => Root;

    private ValueTrie<PermissionActionType> Root { get; }

    protected internal PermissionStore(
        IEnumerable<IReadOnlyDictionary<string, PermissionActionType>> permissionDictionaries)
    {
        Root = new ValueTrie<PermissionActionType>();

        foreach (var permissionDictionary in permissionDictionaries)
        foreach (var (k, v) in permissionDictionary)
        {
            AddPermission(k, v);
        }
    }
}