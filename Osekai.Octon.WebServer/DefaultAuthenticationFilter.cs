using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.Filters;
using Osekai.Octon.Exceptions;
using Osekai.Octon.Permissions;
using Osekai.Octon.Services;

namespace Osekai.Octon.WebServer;

public class DefaultAuthenticationFilter: Attribute, IAsyncAuthorizationFilter
{
    private static readonly Regex Regex = new Regex($@"^osekai_session_(.{{1,{Specifications.SessionTokenLength}}})$");
    
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        AuthenticationService authenticationService = context.HttpContext.RequestServices.GetRequiredService<AuthenticationService>();
        string? token = null;

        try
        {
            if (context.HttpContext.Request.Cookies.TryGetValue("osekai_session_token", out token))
            {
                Match match = Regex.Match(token);

                if (match.Success)
                {
                    PermissionService permissionService = context.HttpContext.RequestServices.GetRequiredService<PermissionService>();
                    CurrentSession currentSession = context.HttpContext.RequestServices.GetService<CurrentSession>()!;

                    AuthenticationService.LogInWithCodeResult result = await authenticationService.LogInWithTokenAsync(match.Groups[1].Value, context.HttpContext.RequestAborted);
                    IPermissionStore permissionStore = await permissionService.GetPermissionStoreAsync(result.OsuSessionContainer.UserId);
                    
                    currentSession.Set(result.OsuSessionContainer, permissionStore);
                }
                else
                    throw new InvalidSessionTokenException(token);
            }
        }
        catch
        {
            context.HttpContext.Response.Cookies.Append("osekai_session_token", string.Empty, new CookieOptions{Expires = DateTimeOffset.MinValue});

            if (token != null)
                await authenticationService.RevokeTokenAsync(token, context.HttpContext.RequestAborted);
        }
    }
}