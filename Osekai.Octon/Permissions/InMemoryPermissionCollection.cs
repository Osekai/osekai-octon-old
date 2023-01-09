using System.Collections;

namespace Osekai.Octon.Permissions;

public class InMemoryPermissionCollection: IPermissionCollection
{
    // It is a binary tree internally -> O(log n) complexity 
    private readonly SortedSet<string> _permissions;

    public void AddPermission(string permission)
    {
        if (!PermissionValidation.IsValidPermission(permission))
            throw new ArgumentException($"The string \"${permission}\" is not a valid permission.");

        IEnumerable<string> matchingPermissions = _permissions
            .Where(p => _permissions.Comparer.Compare(p, permission) == 0).ToArray();

        foreach (var matchingPermission in matchingPermissions)
            _permissions.Remove(matchingPermission);

        _permissions.Add(permission);
    }

    public InMemoryPermissionCollection()
    {
        _permissions = new SortedSet<string>(new PermissionComparer());
    }
    
    public InMemoryPermissionCollection(IEnumerable<string> permissions) : this()
    {
        foreach (var permission in permissions)
            AddPermission(permission);
    }
    
    public bool HasPermission(string permission) => _permissions.Contains(permission);
    public IEnumerator<string> GetEnumerator() => _permissions.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}