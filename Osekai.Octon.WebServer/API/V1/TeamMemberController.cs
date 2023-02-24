using Microsoft.AspNetCore.Mvc;
using Osekai.Octon.Domain.AggregateRoots;
using Osekai.Octon.Services;
using Osekai.Octon.WebServer.API.V1.Dtos.TeamMemberController;

namespace Osekai.Octon.WebServer.API.V1;

public sealed class TeamMemberController: Controller
{
    private readonly TeamMemberService _teamMemberService;
    private readonly IAdapter<TeamMember, TeamMemberDto> _teamMemberDtoAdapter;
    
    public TeamMemberController(TeamMemberService teamMemberService, IAdapter<TeamMember, TeamMemberDto> teamMemberDtoAdapter)
    {
        _teamMemberService = teamMemberService;
        _teamMemberDtoAdapter = teamMemberDtoAdapter;
    }

    [HttpGet("/api/v1/team/")]
    [HttpGet("/home/api/team.php")]
    public async Task<IActionResult> GetTeamMembersAsync(CancellationToken cancellationToken = default)
    {
        IEnumerable<TeamMember> teamMembers = await _teamMemberService.GetTeamMembersAsync(cancellationToken);
        
        return Ok(await teamMembers.ToAsyncEnumerable()
            .SelectAwait(async t => await _teamMemberDtoAdapter.AdaptAsync(t, cancellationToken))
            .ToArrayAsync(cancellationToken));
    }
}