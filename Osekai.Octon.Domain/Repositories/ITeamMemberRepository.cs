using Osekai.Octon.Domain.AggregateRoots;

namespace Osekai.Octon.Domain.Repositories;

public interface ITeamMemberRepository
{
    Task<IEnumerable<TeamMember>> GetTeamMembersAsync(CancellationToken cancellationToken = default);
}