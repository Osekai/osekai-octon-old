namespace Osekai.Octon;

public static class PathUtils
{
    public static string AppendVersionQuery(string @string) => @string + "?v=" + Constants.Version;

    private static readonly Dictionary<string, string> AppendQueryVersionResultCache = new Dictionary<string, string>(); 

    public static string AppendVersionQueryCached(string @string)
    {
        string? result;
        if (AppendQueryVersionResultCache.TryGetValue(@string, out result))
            return result;

        result = AppendVersionQuery(@string);
        AppendQueryVersionResultCache.Add(@string, result);

        return result;
    }
}