using System.Drawing;

namespace Osekai.Octon.Persistence.EntityFramework.MySql;

public static class ColorFormatConversion
{
    public static Color GetColorFromString(string str)
    {
        int r = int.Parse(str.GetSplitSubstringByIndex(',', out int index));
        int g = int.Parse(str.GetSplitSubstringByIndex(',', out index, index + 1));
        int b = int.Parse(str.GetSplitSubstringByIndex(',', out index, index + 1));

        return System.Drawing.Color.FromArgb(r, g, b);
    }
}