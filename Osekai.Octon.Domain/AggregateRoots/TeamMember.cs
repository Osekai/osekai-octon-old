using Osekai.Octon.Domain.ValueObjects;

namespace Osekai.Octon.Domain.AggregateRoots;

public class TeamMember
{
    public TeamMember(int userId, string name, string? nameAlt, string role)
    {
        UserId = userId;
        Name = name;
        NameAlt = nameAlt;
        Role = role;
    }
    
    public int UserId { get; set; }

    private string _name = null!;
    private string? _nameAlt;
    private string _role = null!;
    
    public string Name
    {
        get => _name;
        set
        {
            if (value.Length is < Specifications.TeamMemberNameMinLength or > Specifications.TeamMemberNameMaxLength)
                throw new ArgumentOutOfRangeException(nameof(value), value.Length, $"Invalid {nameof(Name)} length");

            _name = value;  
        }
    }

    public string? NameAlt
    {
        get => _nameAlt;
        set
        {
            if (value == null)
            {
                _nameAlt = null;
                return;
            }

            if (value.Length is < Specifications.TeamMemberNameAltMinLength or > Specifications.TeamMemberNameAltMaxLength)
                throw new ArgumentOutOfRangeException(nameof(value), value.Length, $"Invalid {nameof(NameAlt)} length");

            _nameAlt = value;  
        }
    }

    public string Role
    {
        get => _role; 
        set
        {
            if (value.Length is < Specifications.TeamMemberRoleMinLength or > Specifications.TeamMemberRoleMaxLength)
                throw new ArgumentOutOfRangeException(nameof(value), value.Length, $"Invalid {nameof(Role)} length");

            _role = value;
        }
    }

    public Ref<IReadOnlyList<UserGroup>>? UserGroups { get; set; }
    public Ref<IReadOnlyList<Social>>? Socials { get; set; }
}