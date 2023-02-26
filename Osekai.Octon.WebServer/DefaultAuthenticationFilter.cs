using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.Filters;
using Osekai.Octon.Domain;
using Osekai.Octon.Domain.Services;
using Osekai.Octon.Exceptions;
using Osekai.Octon.Permissions;
using Osekai.Octon.Domain.Services.Default;

namespace Osekai.Octon.WebServer;

public class DefaultAuthenticationFilter: Attribute, IAsyncAuthorizationFilter
{
    private static readonly Regex Regex = new Regex($@"^osekai_session_(.{{1,{Specifications.SessionTokenLength}}})$");
    
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        IAuthenticationService authenticationService = context.HttpContext.RequestServices.GetRequiredService<IAuthenticationService>();
        ILoggerFactory logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>();
        string? token = null;

        try
        {
            if (context.HttpContext.Request.Cookies.TryGetValue("osekai_session_token", out token))
            {
                Match match = Regex.Match(token);

                if (match.Success)
                {
                    IPermissionService permissionService = context.HttpContext.RequestServices.GetRequiredService<IPermissionService>();
                    CurrentSession currentSession = context.HttpContext.RequestServices.GetService<CurrentSession>()!;

                    IAuthenticationService.LogInWithCodeResult result = await authenticationService.LogInWithTokenAsync(match.Groups[1].Value, context.HttpContext.RequestAborted);
                    IPermissionStore permissionStore = await permissionService.GetPermissionStoreAsync(result.OsuSessionContainer.UserId);
                    
                    currentSession.Set(result.OsuSessionContainer, permissionStore);
                }
                else
                    throw new InvalidSessionTokenException(token);
            }
        }
        catch (Exception exception)
        {
            logger.CreateLogger(nameof(DefaultAuthenticationFilter)).LogError(exception, "Could not authenticate");
            context.HttpContext.Response.Cookies.Append("osekai_session_token", string.Empty, new CookieOptions{Expires = DateTimeOffset.MinValue});

            if (token != null)
                await authenticationService.RevokeTokenAsync(token, context.HttpContext.RequestAborted);
        }
    }
}