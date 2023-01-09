namespace Osekai.Octon.Permissions;

public class PermissionComparer: Comparer<string>
{
    private bool _descending;
    
    public PermissionComparer(bool descending = true)
    {
        _descending = descending;
    }
    
    
    public override int Compare(string? x, string? y)
    {
        return CompareInternal(x, y) * (_descending ? -1 : 1);
    }

    private int CompareInternal(string? x, string? y)
    {
        if (ReferenceEquals(x, y)) return 0;
        if (x == null) return -1;
        if (y == null) return 1;

        int startIndexX = -1, startIndexY = -1;

        do
        {
            ReadOnlySpan<char> xSubstring = x.GetSplitSubstringByIndex('.', out startIndexX, startIndexX + 1);
            ReadOnlySpan<char> ySubstring = y.GetSplitSubstringByIndex('.', out startIndexY, startIndexY + 1);

            if (xSubstring.SequenceEqual("*"))
                return 1;
            
            // x is included in y
            if (ySubstring.SequenceEqual("*"))
                return 0;
            
            int cmp = xSubstring.CompareTo(ySubstring, StringComparison.Ordinal);

            if (cmp == 0)
                continue;

            return cmp;
        } while (startIndexX != -1 && startIndexY != -1);

        // cmp was 0, so the permissions where equals
        return 0;
    }

}