using Microsoft.AspNetCore.Mvc;
using Osekai.Octon.Services;

namespace Osekai.Octon.WebServer.API;

[DefaultAuthenticationFilter]
public class LoginController: Controller
{
    private readonly AuthenticationService _authenticationService;
    
    public LoginController(AuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpGet("/login")]
    public async Task<IActionResult> Login([FromQuery] string code, CancellationToken cancellationToken)
    {
        var (_, token, expiresAt) = await _authenticationService.SignInWithCodeAsync(code, cancellationToken);
        
        Response.Cookies.Append(
            new [] { new KeyValuePair<string,string>("osekai_session_token", $"osekai_session_{token}") }, 
            new CookieOptions{ HttpOnly = true, Expires = expiresAt });
        
        return Redirect("/");
    }
}