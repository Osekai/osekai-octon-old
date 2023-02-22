namespace Osekai.Octon.Persistence.EntityFramework.MySql.Entities
{
    internal sealed class HomeFaq
    {
        public int Id { get; set; }
        public int AppId { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string LocalizationPrefix { get; set; } = null!;

        public App App { get; set; } = null!;

        public Domain.Aggregates.HomeFaq ToValueObject()
        {
            return new Domain.Aggregates.HomeFaq(Id, AppId, Title, Content, LocalizationPrefix);
        }
    }
}
