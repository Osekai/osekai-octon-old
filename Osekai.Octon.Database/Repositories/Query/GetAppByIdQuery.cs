namespace Osekai.Octon.Database.Repositories.Query;

public readonly struct GetAppByIdQuery
{
    public GetAppByIdQuery(int id, bool includeTheme = false)
    {
        Id = id;
        IncludeTheme = includeTheme;
    }

    public int Id { get; }
    public bool IncludeTheme { get; }
}