using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Osekai.Octon.Domain.AggregateRoots;
using Osekai.Octon.OsuApi;
using Osekai.Octon.OsuApi.Payloads;
using Osekai.Octon.Services;
using Osekai.Octon.WebServer.API.V1.Dtos.UserController;

namespace Osekai.Octon.WebServer.API.V1;

[DefaultAuthenticationFilter]
public sealed class UserController: Controller
{
    private readonly IAuthenticatedOsuApiV2Interface _osuApiV2Interface;
    private readonly CachedAuthenticatedOsuApiV2Interface _cachedAuthenticatedOsuApiV2Interface;
    private readonly CurrentSession _currentSession;
    private readonly IAdapter<(OsuUser, IEnumerable<UserGroup>), UserDto> _userDtoFromOsuUserAndGroupsAdapter;
    private readonly UserGroupService _userGroupService;
    
    public UserController(CurrentSession currentSession, 
        IAuthenticatedOsuApiV2Interface osuApiV2Interface,
        UserGroupService userGroupService,
        CachedAuthenticatedOsuApiV2Interface cachedAuthenticatedOsuApiV2Interface,
        IAdapter<(OsuUser, IEnumerable<UserGroup>), UserDto> userDtoFromOsuUserAndGroupsAdapter)
    {
        _userGroupService = userGroupService;
        _osuApiV2Interface = osuApiV2Interface;
        _cachedAuthenticatedOsuApiV2Interface = cachedAuthenticatedOsuApiV2Interface;
        _userDtoFromOsuUserAndGroupsAdapter = userDtoFromOsuUserAndGroupsAdapter;
        _currentSession = currentSession;
    }
    
    [HttpGet("/api/v1/users/{userId:int}")]
    [HttpGet("/api/profiles/get_user.php")]
    public async Task<IActionResult> GetUserByIdAsync(
        [FromQuery(Name = "id")] int? queryUserId, int userId,
        CancellationToken cancellationToken)
    {
        if (queryUserId != null)
            userId = queryUserId.Value;

        OsuUser? osuUser = await _cachedAuthenticatedOsuApiV2Interface.SearchUserAsync(_currentSession, userId.ToString(), cancellationToken: cancellationToken);
        
        if (osuUser == null)
            return NotFound();

        IEnumerable<UserGroup> userGroups = await _userGroupService.GetUserGroupsOfUserAsync(userId, cancellationToken);
        
        return Ok(await _userDtoFromOsuUserAndGroupsAdapter.AdaptAsync((osuUser, userGroups), cancellationToken));
    }
}