using Osekai.Octon.Database.EntityFramework;

namespace Osekai.Octon.Database.Models
{
    public sealed class AppTheme
    {
        public int Id { get; set; }
        public int AppId { get; set; }
        public string Name { get; set; } = null!;
        public string Color { get; set; } = null!;
        public float HslValueMultiplier { get; set; }
        public sbyte HasCover { get; set; }

        public App App { get; set; } = null!;
    }
}
