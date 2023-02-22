namespace Osekai.Octon.Drawing;

public readonly struct HslColour: IEquatable<HslColour>
{
    public HslColour(double h, double s, double l)
    {
        H = h;
        S = s;
        L = l;
    }

    public double H { get; }
    public double S { get; }
    public double L { get; }

    public bool Equals(HslColour other)
    {
        return H.Equals(other.H) && S.Equals(other.S) && L.Equals(other.L);
    }

    public override bool Equals(object? obj)
    {
        return obj is HslColour other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(H, S, L);
    }

    public static bool operator ==(HslColour left, HslColour right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(HslColour left, HslColour right)
    {
        return !left.Equals(right);
    }
}