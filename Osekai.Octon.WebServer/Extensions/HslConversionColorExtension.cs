using System.Drawing;
using Osekai.Octon.WebServer.Drawing;

namespace Osekai.Octon.WebServer.Extensions;

public static class HslConversionColorExtensions
{
    //Adapted from https://github.com/Osekai/osekai/blob/main/global/php/functions.php#L776
    public static Hsl ToHsl(this Color color)
    {
        byte r = color.R;
        byte g = color.G;
        byte b = color.B;
        
        int min = Math.Min(Math.Min(r, g), b);
        int max = Math.Max(Math.Max(r, g), b);
        int delta = max - min;

        double h = 0;
        double s = 0;
        
        double maxd = max / 255d;
        double mind = min / 255d;
        
        double l = ((maxd + mind) / 2.0);

        if (delta != 0)
        {
            double rd = r / 255d;
            double gd = g / 255d;
            double bd = b / 255d;
            double ddelta = delta / 255d;

            s = ddelta / (1 - Math.Abs(2 * l - 1));
            
            if (r == max)
            {
                h = 60d * Math.IEEERemainder((gd - bd) / ddelta, 6);
                if (b > g)
                    h += 360d;
            }
            else if (g == max)
            {
                h = 60d * ((bd - rd) / ddelta + 2);
            }
            else if (b == max)
            {
                h = 60d * ((rd - gd) / ddelta + 4);
            }
        }

        return new Hsl(Math.Round(h,2),Math.Round(s * 100,2),Math.Round(l * 100, 2));
    }
}