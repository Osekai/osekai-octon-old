using System.Text.RegularExpressions;

namespace Osekai.Octon.Permissions;

public static class PermissionValidation
{
    private static readonly Regex ValidPermissionRegex = new Regex(@"^(?:[^\*\.]+\.?)*(?:[^\.]|(?:\*)+)$");

}