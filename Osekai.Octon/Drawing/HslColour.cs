namespace Osekai.Octon.Drawing;

public readonly struct HslColour
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
}