using Osekai.Octon.Domain.AggregateRoots;

namespace Osekai.Octon.Domain.Services;

public interface ITeamMemberService
{
    Task<IEnumerable<TeamMember>> GetTeamMembersAsync(CancellationToken cancellationToken = default);
}