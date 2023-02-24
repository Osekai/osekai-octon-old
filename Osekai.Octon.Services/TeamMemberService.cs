using Osekai.Octon.Domain.AggregateRoots;
using Osekai.Octon.Domain.Repositories;
using Osekai.Octon.Persistence;

namespace Osekai.Octon.Services;

public class TeamMemberService: ITeamMemberRepository
{
    private readonly IUnitOfWork _unitOfWork;
    
    public TeamMemberService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public Task<IEnumerable<TeamMember>> GetTeamMembersAsync(CancellationToken cancellationToken = default) =>
        _unitOfWork.TeamMemberRepository.GetTeamMembersAsync(cancellationToken);
}