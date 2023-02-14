using Osekai.Octon.Persistence.EntityFramework.MySql.Dtos;

namespace Osekai.Octon.Persistence.EntityFramework.MySql.Entities
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



        public ReadOnlyAppThemeDto ToDto()
        {
            return new ReadOnlyAppThemeDto(Id, 
                ColorFormatConversion.GetColorFromString(Color), ColorFormatConversion.GetColorFromString(DarkColor), HasCover, 
                HslValueMultiplier, DarkHslValueMultiplier);
        }
    }
}
