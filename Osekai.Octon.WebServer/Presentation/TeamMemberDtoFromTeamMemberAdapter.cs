using Osekai.Octon.Domain.AggregateRoots;
using Osekai.Octon.WebServer.API.V1.Dtos.TeamMemberController;

namespace Osekai.Octon.WebServer.Presentation;

public class TeamMemberDtoFromTeamMemberAdapter: IAdapter<TeamMember, TeamMemberDto>
{
    public ValueTask<TeamMemberDto> AdaptAsync(TeamMember value, CancellationToken cancellationToken = default)
    {
        return ValueTask.FromResult(new TeamMemberDto
        {
            Name = value.Name,
            Role = value.Role,
            Socials = value.Socials!.Value.Value.Select(s => new TeamMemberDtoSocial{Icon = s.Icon, Link = s.Link, Name = s.Name}).ToArray(),
            NameAlt = value.NameAlt,
            UserId = value.UserId,
            UserGroupIds = value.UserGroups!.Value.Value.Select(s => s.Id).ToArray()
        });
    }
}