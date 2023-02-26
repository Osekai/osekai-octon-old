using Microsoft.AspNetCore.Mvc;
using Osekai.Octon.Domain.Services;
using Osekai.Octon.Persistence;
using Osekai.Octon.Domain.Services.Default;

namespace Osekai.Octon.WebServer.API;

[DefaultAuthenticationFilter]
public sealed class LoginController: Controller
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly StaticUrlGenerator _staticUrlGenerator;
    
    public LoginController(IUnitOfWork unitOfWork, StaticUrlGenerator staticUrlGenerator, IAuthenticationService authenticationService)
    {
        _unitOfWork = unitOfWork;
        _staticUrlGenerator = staticUrlGenerator;
        _authenticationService = authenticationService;
    }

    [HttpGet("/login")]
    public async Task<IActionResult> Login([FromQuery] string? code, CancellationToken cancellationToken)
    {
        if (code == null)
            return Redirect(_staticUrlGenerator.Get(StaticUrlGenerator.StaticUrlGeneratorString.OsuLoginString));
        
        IAuthenticationService.SignInWithCodeResult result = await _authenticationService.SignInWithCodeAsync(code, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        Response.Cookies.Append(
            new [] { new KeyValuePair<string,string>("osekai_session_token", $"osekai_session_{result.Token}") }, 
            new CookieOptions{ HttpOnly = true, Expires = result.ExpiresAt });
        
        return Redirect("/");
    }
}