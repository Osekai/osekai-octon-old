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
        AuthenticationService.SignInWithCodeResult result = await _authenticationService.SignInWithCodeAsync(code, cancellationToken);
        
        Response.Cookies.Append(
            new [] { new KeyValuePair<string,string>("osekai_session_token", $"osekai_session_{result.Token}") }, 
            new CookieOptions{ HttpOnly = true, Expires = result.ExpiresAt });
        
        return Redirect("/");
    }
}