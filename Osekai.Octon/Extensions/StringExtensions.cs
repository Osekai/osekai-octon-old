﻿namespace Osekai.Octon.Extensions;

public static class StringExtensions
{
    public static ReadOnlySpan<char> GetSplitSubstringByIndex(this string str, char delimiter, out int index,  int startIndex = 0)
    {
        index = str.IndexOf(delimiter, startIndex);
        if (index < 0)
            return str.AsSpan(startIndex, str.Length - startIndex);

        return str.AsSpan(startIndex, index - startIndex);
    }
}