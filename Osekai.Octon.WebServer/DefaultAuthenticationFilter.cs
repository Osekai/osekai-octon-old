using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Osekai.Octon;
using Osekai.Octon.Database;
using Osekai.Octon.Database.Dtos;
using Osekai.Octon.Exceptions;
using Osekai.Octon.OsuApi;
using Osekai.Octon.OsuApi.Payloads;
using Osekai.Octon.Services;

namespace Osekai.Octon.WebServer;

public class DefaultAuthenticationFilter: Attribute, IAsyncAuthorizationFilter
{
    private static readonly Regex Regex = new Regex($@"^osekai_session_(.{{1,{Specifications.SessionTokenLength}}})$");
    
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        try
        {
            if (context.HttpContext.Request.Cookies.TryGetValue("osekai_session_token", out string? token))
            {
                Match match = Regex.Match(token);

                if (match.Success)
                {
                    AuthenticationService authenticationService = context.HttpContext.RequestServices.GetService<AuthenticationService>()!;
                    SessionService sessionService = context.HttpContext.RequestServices.GetService<SessionService>()!;
                    
                    SessionDto session = await authenticationService.LogInWithTokenAsync(match.Groups[1].Value, context.HttpContext.RequestAborted);

                    CurrentSession currentSession = context.HttpContext.RequestServices.GetService<CurrentSession>()!;
                    currentSession.Set(session);

                    CachedAuthenticatedOsuApiV2Interface cachedAuthenticatedOsuApiV2Interface = context.HttpContext.RequestServices.GetService<CachedAuthenticatedOsuApiV2Interface>()!;
                    OsuUser user = await cachedAuthenticatedOsuApiV2Interface.MeAsync(currentSession, cancellationToken: context.HttpContext.RequestAborted);

                    if (user.IsRestricted)
                        throw new OsuUserRestrictedException(user.Id);

                    if (user.IsDeleted)
                        throw new OsuUserDeletedException(user.Id);
                }
                else
                    throw new InvalidSessionTokenException(token);
            }
        }
        catch
        {
            context.HttpContext.Response.Cookies.Append("osekai_session_token", string.Empty, new CookieOptions{Expires = DateTimeOffset.MinValue});
        }
    }
}