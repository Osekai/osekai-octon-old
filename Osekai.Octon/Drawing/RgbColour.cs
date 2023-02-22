namespace Osekai.Octon.Drawing;

public readonly struct RgbColour
{
    public RgbColour(byte r, byte g, byte b)
    {
        R = r;
        G = g;
        B = b;
    } 
    
    public byte R { get; }
    public byte G { get; }
    public byte B { get; }
}