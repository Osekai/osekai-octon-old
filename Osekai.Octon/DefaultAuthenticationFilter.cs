﻿using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Osekai.Octon.Applications;
using Osekai.Octon.Services;
using Osekai.Octon.Services.Query;

namespace Osekai.Octon;

public class DefaultAuthenticationFilter: Attribute, IAsyncAuthorizationFilter
{
    private static readonly Regex Regex = new Regex(@"^osekai_session_(.+)$");
    
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        if (context.HttpContext.Request.Cookies.TryGetValue("osekai_session_token", out string? token))
        {
            Match match = Regex.Match(token);
            if (match.Success)
            {
                AuthenticationService authenticationService =
                    context.HttpContext.RequestServices.GetService<AuthenticationService>()!;

               await authenticationService.LogInWithTokenAsync(new AuthenticateWithTokenQuery(match.Groups[1].Value), 
                   context.HttpContext.RequestAborted);
            }
        }
    }
}