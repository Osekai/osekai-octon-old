using System.Text.RegularExpressions;

namespace Osekai.Octon.Permissions;

public interface IPermissionCollection: IEnumerable<string>
{
    void AddPermission(string permission);
    bool HasPermission(string permission);
}