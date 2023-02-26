using Osekai.Octon.Domain.ValueObjects;
using Osekai.Octon.Persistence.EntityFramework.MySql.Serializables;

namespace Osekai.Octon.Persistence.EntityFramework.MySql.Entities;

internal class TeamMember
{
    public int UserId { get; set; }
    public string Name { get; set; } = null!;
    public string? NameAlt { get; set; }
    public string Role { get; set; } = null!;

    public ICollection<SerializableSocial> Socials { get; set; } = null!;

    public Domain.AggregateRoots.TeamMember ToAggregateRoot()
    {
        return new Domain.AggregateRoots.TeamMember(UserId, Name, NameAlt, Role);
    }
}  