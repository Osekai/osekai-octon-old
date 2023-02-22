using System.Drawing;
using Osekai.Octon.Drawing;
using Osekai.Octon.Extensions;

namespace Osekai.Octon.Persistence.EntityFramework.MySql;

public static class ColorFormatConversion
{
    public static RgbColour GetColorFromString(string str)
    {
        int r = int.Parse(str.GetSplitSubstringByIndex(',', out int index));
        int g = int.Parse(str.GetSplitSubstringByIndex(',', out index, index + 1));
        int b = int.Parse(str.GetSplitSubstringByIndex(',', out index, index + 1));

        return new RgbColour((byte)r, (byte)g, (byte)b);
    }
}