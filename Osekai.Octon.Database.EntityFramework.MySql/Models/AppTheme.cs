using System.Drawing;
using Osekai.Octon.Database.Dtos;

namespace Osekai.Octon.Database.EntityFramework.MySql.Models
{
    internal sealed class AppTheme
    {
        public int Id { get; set; }
        public int AppId { get; set; }
        public string Color { get; set; } = null!;
        public string DarkColor { get; set; } = null!;
        public float HslValueMultiplier { get; set; }
        public float DarkHslValueMultiplier { get; set; }
        public bool HasCover { get; set; }
        public App App { get; set; } = null!;

        private static Color GetColorFromString(string str)
        {
            int r = int.Parse(str.GetSplitSubstringByIndex(',', out int index));
            int g = int.Parse(str.GetSplitSubstringByIndex(',', out index, index));
            int b = int.Parse(str.GetSplitSubstringByIndex(',', out index, index));

            return System.Drawing.Color.FromArgb(r, g, b);
        }

        public AppThemeDto ToDto()
        {
            return new AppThemeDto(Id, 
                GetColorFromString(Color), GetColorFromString(DarkColor), HasCover, 
                HslValueMultiplier, DarkHslValueMultiplier);
        }
    }
}
