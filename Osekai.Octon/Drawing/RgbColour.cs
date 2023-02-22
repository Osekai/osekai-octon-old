namespace Osekai.Octon.Drawing;

public readonly struct RgbColour: IEquatable<RgbColour>
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

    public bool Equals(RgbColour other)
    {
        return R == other.R && G == other.G && B == other.B;
    }

    public override bool Equals(object? obj)
    {
        return obj is RgbColour other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(R, G, B);
    }

    public static bool operator ==(RgbColour left, RgbColour right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(RgbColour left, RgbColour right)
    {
        return !left.Equals(right);
    }
}