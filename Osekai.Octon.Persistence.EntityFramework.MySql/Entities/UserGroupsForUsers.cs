namespace Osekai.Octon.Persistence.EntityFramework.MySql.Entities;

internal sealed class UserGroupsForUsers
{
    public int UserId { get; set; }
    public int UserGroupId { get; set; }
    public UserGroup UserGroup { get; set; } = null!;
}