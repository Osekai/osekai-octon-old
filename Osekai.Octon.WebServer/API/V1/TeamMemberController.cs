using Microsoft.AspNetCore.Mvc;
using Osekai.Octon.Domain.AggregateRoots;
using Osekai.Octon.Domain.Services;
using Osekai.Octon.WebServer.API.V1.Dtos.TeamMemberController;

namespace Osekai.Octon.WebServer.API.V1;

public sealed class TeamMemberController: Controller
{
    private readonly ITeamMemberService _teamMemberService;
    private readonly IConverter<TeamMember, TeamMemberDto> _teamMemberDtoConverter;
    
    public TeamMemberController(ITeamMemberService teamMemberService, IConverter<TeamMember, TeamMemberDto> teamMemberDtoConverter)
    {
        _teamMemberService = teamMemberService;
        _teamMemberDtoConverter = teamMemberDtoConverter;
    }

    [HttpGet("/api/v1/team/")]
    [HttpGet("/home/api/team.php")]
    public async Task<IActionResult> GetTeamMembersAsync(CancellationToken cancellationToken = default)
    {
        IEnumerable<TeamMember> teamMembers = await _teamMemberService.GetTeamMembersAsync(cancellationToken);
        
        return Ok(await teamMembers.ToAsyncEnumerable()
            .SelectAwait(async t => await _teamMemberDtoConverter.ConvertAsync(t, cancellationToken))
            .ToArrayAsync(cancellationToken));
    }
}