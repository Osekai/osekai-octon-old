using Osekai.Octon.Persistence.EntityFramework.MySql.Dtos;

namespace Osekai.Octon.Persistence.EntityFramework.MySql.Entities
{
    internal sealed class App
    {
        public int Id { get; set; }
        public int Order { get; set; }
        public string Name { get; set; } = null!;
        public string SimpleName { get; set; } = null!;
        public bool Visible { get; set; }
        public bool Experimental { get; set; }

        public AppTheme? AppTheme { get; set; }

        public AppDto ToDto() =>
            new AppDto(Id, Order, Name, SimpleName, Visible, Experimental, AppTheme?.ToDto());
    }
}
