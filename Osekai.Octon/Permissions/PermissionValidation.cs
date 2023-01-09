using System.Text.RegularExpressions;

namespace Osekai.Octon.Permissions;

public static class PermissionValidation
{
    private static readonly Regex ValidPermissionRegex = new Regex(@"^(?:[^\*\.]+\.?)*(?:[^\.]|(?:\*)+)$");

    public static bool IsValidPermission(string permission) => ValidPermissionRegex.IsMatch(permission);
}