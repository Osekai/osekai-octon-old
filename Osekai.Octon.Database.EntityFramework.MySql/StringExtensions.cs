namespace Osekai.Octon.Database.EntityFramework.MySql;

public static class StringExtensions
{
    internal static ReadOnlySpan<char> GetSplitSubstringByIndex(this string str, char delimiter, out int index,  int startIndex = 0)
    {
        index = str.IndexOf(delimiter, startIndex);
        if (index < 0)
            return str.AsSpan(startIndex, str.Length - startIndex);

        return str.AsSpan(startIndex, index - startIndex);
    }
}