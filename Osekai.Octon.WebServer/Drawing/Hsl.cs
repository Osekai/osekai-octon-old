namespace Osekai.Octon.WebServer.Drawing;

public readonly struct Hsl
{
    public Hsl(double h, double s, double l)
    {
        H = h;
        S = s;
        L = l;
    }

    public double H { get; }
    public double S { get; }
    public double L { get; }
}