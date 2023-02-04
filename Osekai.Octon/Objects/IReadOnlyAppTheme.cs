using System.Drawing;

namespace Osekai.Octon.Objects;

public interface IReadOnlyAppTheme
{
    int Id { get; }
    Color Color { get; } 
    Color DarkColor { get; } 
        
    float HslValueMultiplier { get; }
    float DarkHslValueMultiplier { get; }

    bool HasCover { get; }
}