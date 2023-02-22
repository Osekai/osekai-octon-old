using System.Drawing;
using Osekai.Octon.Drawing;

namespace Osekai.Octon.Domain.Entities;

public class AppTheme
{
    public AppTheme(int id, RgbColour color, RgbColour darkColor, float hslValueMultiplier, float darkHslValueMultiplier, bool hasCover)
    {
        Id = id;
        Color = color;
        DarkColor = darkColor;
        HslValueMultiplier = hslValueMultiplier;
        DarkHslValueMultiplier = darkHslValueMultiplier;
        HasCover = hasCover;
    }

    public int Id { get; }
    public RgbColour Color { get; set; } 
    public RgbColour DarkColor { get; set; } 
        
    public float HslValueMultiplier { get; set; }
    public float DarkHslValueMultiplier { get; set; }

    public bool HasCover { get; set; }
}