using Osekai.Octon.Drawing;

namespace Osekai.Octon.Domain.ValueObjects;

public readonly struct AppTheme
{
    public AppTheme(RgbColour color, RgbColour darkColor, float hslValueMultiplier, float darkHslValueMultiplier, bool hasCover)
    {
        Color = color;
        DarkColor = darkColor;
        HslValueMultiplier = hslValueMultiplier;
        DarkHslValueMultiplier = darkHslValueMultiplier;
        HasCover = hasCover;
    }

    public RgbColour Color { get; } 
    public RgbColour DarkColor { get; } 
        
    public float HslValueMultiplier { get; }
    public float DarkHslValueMultiplier { get; }

    public bool HasCover { get; }
}