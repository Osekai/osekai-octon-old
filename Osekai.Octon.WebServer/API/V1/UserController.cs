using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Osekai.Octon.OsuApi;
using Osekai.Octon.OsuApi.Payloads;
using Osekai.Octon.Query;
using Osekai.Octon.Query.QueryParams;
using Osekai.Octon.Query.QueryResults;
using Osekai.Octon.WebServer.API.V1.Dtos.UserController;

namespace Osekai.Octon.WebServer.API.V1;

[DefaultAuthenticationFilter]
public sealed class UserController: Controller
{
    private readonly IAuthenticatedOsuApiV2Interface _osuApiV2Interface;
    private readonly CachedAuthenticatedOsuApiV2Interface _cachedAuthenticatedOsuApiV2Interface;
    private readonly CurrentSession _currentSession;
    private readonly IAdapter<(OsuUser, IReadOnlyUserAggregateQueryResult), UserDto> _userDtoFromOsuUserAndAggregateAdapter;
    private readonly IParameterizedQuery<IReadOnlyUserAggregateQueryResult, UserAggregateParam> _userAggregateQuery;
    
    public UserController(CurrentSession currentSession, 
        IAuthenticatedOsuApiV2Interface osuApiV2Interface,
        CachedAuthenticatedOsuApiV2Interface cachedAuthenticatedOsuApiV2Interface,
        IParameterizedQuery<IReadOnlyUserAggregateQueryResult, UserAggregateParam> userAggregateQuery,
        IAdapter<(OsuUser, IReadOnlyUserAggregateQueryResult), UserDto> userDtoFromOsuUserAndAggregateAdapter)
    {
        _userDtoFromOsuUserAndAggregateAdapter = userDtoFromOsuUserAndAggregateAdapter;
        _osuApiV2Interface = osuApiV2Interface;
        _cachedAuthenticatedOsuApiV2Interface = cachedAuthenticatedOsuApiV2Interface;
        _currentSession = currentSession;
        _userAggregateQuery = userAggregateQuery;
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

        IReadOnlyUserAggregateQueryResult userAggregateQueryResult = await _userAggregateQuery.ExecuteAsync(new UserAggregateParam(userId), cancellationToken);
        
        return Ok(await _userDtoFromOsuUserAndAggregateAdapter.AdaptAsync((osuUser, userAggregateQueryResult), cancellationToken));
    }
}