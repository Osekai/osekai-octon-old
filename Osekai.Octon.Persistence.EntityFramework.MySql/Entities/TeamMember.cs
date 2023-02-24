using Osekai.Octon.Domain.ValueObjects;

namespace Osekai.Octon.Persistence.EntityFramework.MySql.Entities;

internal class TeamMember
{
    public int UserId { get; set; }
    public string Name { get; set; } = null!;
    public string? NameAlt { get; set; }
    public string Role { get; set; } = null!;

    public ICollection<Social> Socials { get; set; } = null!;

    public Domain.AggregateRoots.TeamMember ToAggregateRoot()
    {
        return new Domain.AggregateRoots.TeamMember(UserId, Name, NameAlt, Role);
    }
}  