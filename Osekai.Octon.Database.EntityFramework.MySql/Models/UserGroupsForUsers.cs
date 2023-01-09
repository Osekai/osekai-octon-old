using Osekai.Octon.Database.HelperTypes;

namespace Osekai.Octon.Database.EntityFramework.MySql.Models;

public class UserGroupsForUsers
{
    public int UserId { get; set; }
    public int UserGroupId { get; set; }
    public UserGroup UserGroup { get; set; } = null!;
}