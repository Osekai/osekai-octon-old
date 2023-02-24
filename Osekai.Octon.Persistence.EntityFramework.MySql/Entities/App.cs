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

        public Domain.AggregateRoots.App ToAggregateRoot(bool includeObjects = false)
        {
            Domain.AggregateRoots.App app = new Domain.AggregateRoots.App(Name, SimpleName, Id, Order, Visible, Experimental);
            return app;
        }

        public IList<HomeFaq> Faqs { get; } = null!;
    }
}
