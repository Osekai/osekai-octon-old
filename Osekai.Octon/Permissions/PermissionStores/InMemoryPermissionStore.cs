﻿using Osekai.Octon.DataStructures.Trie;

namespace Osekai.Octon.Permissions.PermissionStores;

public class InMemoryPermissionStore: IPermissionStore
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

    public ValueTask<bool> HasPermissionAsync(string path)
        => ValueTask.FromResult(HasPermission(path));

    public IEnumerable<KeyValuePair<string, PermissionActionType>> GetPermissions()
        => Root;

    public ValueTask<IEnumerable<KeyValuePair<string, PermissionActionType>>> GetPermissionsAsync()
        => ValueTask.FromResult(GetPermissions());

    private ValueTrie<PermissionActionType> Root { get; }

    public InMemoryPermissionStore(
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