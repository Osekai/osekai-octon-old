using System.Drawing;

namespace Osekai.Octon.RichModels;

public class AppTheme
{
    public AppTheme(
        int id = default, 
        Color color = default, 
        Color darkColor = default, 
        float hslValueMultiplier = default, 
        float darkHslValueMultiplier = default,
        bool hasCover = default)
    {
        Id = id;
        Color = color;
        DarkColor = darkColor;
        HslValueMultiplier = hslValueMultiplier;
        DarkHslValueMultiplier = darkHslValueMultiplier;
        HasCover = hasCover;
    }

    public int Id { get; }
    public Color Color { get; set; } 
    public Color DarkColor { get; set; } 
        
    public float HslValueMultiplier { get; set; }
    public float DarkHslValueMultiplier { get; set; }

    public bool HasCover { get; set; }
}