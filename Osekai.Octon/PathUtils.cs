namespace Osekai.Octon;

public static class PathUtils
{
    public static string AppendQueryVersion(string @string) => @string + "?v=" + Constants.Version;

    private static readonly Dictionary<string, string> AppendQueryVersionResultCache = new Dictionary<string, string>(); 

    public static string AppendQueryVersionCached(string @string)
    {
        string? result;
        if (AppendQueryVersionResultCache.TryGetValue(@string, out result))
            return result;

        result = AppendQueryVersion(@string);
        AppendQueryVersionResultCache.Add(@string, result);

        return result;
    }
}