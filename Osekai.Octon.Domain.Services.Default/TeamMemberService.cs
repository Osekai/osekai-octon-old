using Osekai.Octon.Domain.AggregateRoots;
using Osekai.Octon.Domain.Repositories;
using Osekai.Octon.Domain.Services;
using Osekai.Octon.Persistence;

namespace Osekai.Octon.Domain.Services.Default;

public class TeamMemberService: ITeamMemberService
{
    private readonly IUnitOfWork _unitOfWork;
    
    public TeamMemberService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public Task<IEnumerable<TeamMember>> GetTeamMembersAsync(CancellationToken cancellationToken = default) =>
        _unitOfWork.TeamMemberRepository.GetTeamMembersAsync(cancellationToken);
}