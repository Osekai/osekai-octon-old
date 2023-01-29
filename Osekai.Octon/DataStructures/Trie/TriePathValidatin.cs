using System.Text.RegularExpressions;

namespace Osekai.Octon.DataStructures.Trie;

public class TriePathValidatin
{
    private static readonly Regex ValidPermissionRegex = new Regex(@"^(?:[^\*\.]+\.?)*(?:[^\.]|(?:\*)+)$");
    
    public static bool IsValidPath(string path) => ValidPermissionRegex.IsMatch(path);

}