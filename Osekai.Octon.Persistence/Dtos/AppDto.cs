namespace Osekai.Octon.Persistence.Dtos;

public class AppDto
{
    public AppDto(int id, int order, string name, string simpleName, 
        bool visible = true, bool experimental = false, AppThemeDto? appThemeDto = null)
    {
        Id = id;
        Order = order;
        Name = name;
        SimpleName = simpleName;
        Visible = visible;
        AppTheme = appThemeDto;
        Experimental = experimental;
    }
    
    public int Id { get; }
    public int Order { get; }
    public string Name { get; } 
    public string SimpleName { get; }
    public bool Visible { get; }
    public bool Experimental { get; }
    public AppThemeDto? AppTheme { get; }
}