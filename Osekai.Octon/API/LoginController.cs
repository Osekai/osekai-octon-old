﻿using Microsoft.AspNetCore.Mvc;
using Osekai.Octon.Applications;
using Osekai.Octon.Database;
using Osekai.Octon.Database.Models;
using Osekai.Octon.Exceptions;
using Osekai.Octon.Services;
using Osekai.Octon.Services.Query;

namespace Osekai.Octon.API;

[DefaultAuthenticationFilter]
public class LoginController: Controller
{
    private readonly AuthenticationService _authenticationService;
    private readonly CurrentSession _currentSession;
    
    public LoginController(CurrentSession currentSession, AuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
        _currentSession = currentSession;
    }

    [HttpGet("/login")]
    public async Task<IActionResult> Login([FromQuery] string code, CancellationToken cancellationToken)
    {
        Session session = await _authenticationService.SignInWithCodeAsync(new AuthenticationWithCodeQuery(code), cancellationToken);
        
        Response.Cookies.Append(
            new [] { new KeyValuePair<string,string>("osekai_session_token", $"osekai_session_{session.Token}") }, 
            new CookieOptions{ HttpOnly = true, Expires = session.ExpiresAt });
        
        return Redirect("/");
    }
}