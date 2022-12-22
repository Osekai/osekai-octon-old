using Osekai.Octon.Database.EntityFramework;

namespace Osekai.Octon.Database.Models
{
    public sealed class AppTheme
    {
        public int Id { get; set; }
        public int AppId { get; set; }
        public string Color { get; set; } = null!;
        public string DarkColor { get; set; } = null!;
        
        public float HslValueMultiplier { get; set; }
        public float DarkHslValueMultiplier { get; set; }

        public bool HasCover { get; set; }

        public App App { get; set; } = null!;
    }
}
