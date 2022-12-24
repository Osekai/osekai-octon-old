namespace Osekai.Octon.Database.Models
{
    public sealed class App
    {
        public int Id { get; set; }
        public int Order { get; set; }
        public string Name { get; set; } = null!;
        public string SimpleName { get; set; } = null!;
        public bool Visible { get; set; }
        public bool Experimental { get; set; }

        public AppTheme? AppTheme { get; set; }
    }
}
