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

        public Domain.Aggregates.App ToAggregate(bool includeObjects = false)
        {
            Domain.Aggregates.App app = new Domain.Aggregates.App(Name, SimpleName, Id, Order, Visible, Experimental);
            return app;
        }
    }
}
