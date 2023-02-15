using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Osekai.Octon.OsuApi;
using Osekai.Octon.OsuApi.Payloads;
using Osekai.Octon.WebServer.API.V1.Dtos.UserController;

namespace Osekai.Octon.WebServer.API.V1;

[DefaultAuthenticationFilter]
public sealed class UserController: Controller
{
    private readonly IAuthenticatedOsuApiV2Interface _osuApiV2Interface;
    private readonly CurrentSession _currentSession;
    
    public UserController(CurrentSession currentSession, IAuthenticatedOsuApiV2Interface osuApiV2Interface)
    {
        _osuApiV2Interface = osuApiV2Interface;
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
        
        OsuUser? osuUser = await _osuApiV2Interface.SearchUserAsync(_currentSession, userId.ToString(), cancellationToken: cancellationToken);

        if (osuUser == null)
            return NotFound();

        // needs to be converted to a DTO
        return Ok(osuUser);
    }
}