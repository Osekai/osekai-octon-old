using System.Drawing;

namespace Osekai.Octon.Database.Dtos;

public class AppThemeDto
{
    public AppThemeDto(int id, Color color, Color darkColor, bool hasCover,
        float hslValueMultiplier = 1, float darkHslValueMultiplier = 1)
    {
        Id = id;
        Color = color;
        DarkColor = darkColor;
        HasCover = hasCover;
        HslValueMultiplier = hslValueMultiplier;
        DarkHslValueMultiplier = darkHslValueMultiplier;
    }
    
    public int Id { get; }
    public Color Color { get; } 
    public Color DarkColor { get; } 
        
    public float HslValueMultiplier { get; }
    public float DarkHslValueMultiplier { get; }

    public bool HasCover { get; }
}