using Microsoft.AspNetCore.Mvc;
using Osekai.Octon.Enums;
using Osekai.Octon.Exceptions;
using Osekai.Octon.OsuApi;
using Osekai.Octon.Persistence;
using Osekai.Octon.Services;
using Osekai.Octon.Services.Entities;
using Osekai.Octon.Services.PermissionStores;
using Osekai.Octon.WebServer.Presentation.AppBaseLayout;

namespace Osekai.Octon.WebServer.API.V1;

#if DEBUG
[Route("/api/v1/dev")]
[DefaultAuthenticationFilter]
public class DevController: Controller
{
    private readonly ITestDataPopulator _testDataPopulator;
    private readonly StaticUrlGenerator _staticUrlGenerator;
    private readonly CurrentSession _currentSession;
    private readonly CachedAuthenticatedOsuApiV2Interface _authenticatedOsuApiV2Interface;
    private readonly CachedAppBaseLayoutMedalDataGenerator _appBaseLayoutMedalDataGenerator;
    private readonly PermissionService _permissionService;
    
    public DevController(StaticUrlGenerator staticUrlGenerator,
        CurrentSession currentSession,
        CachedAuthenticatedOsuApiV2Interface authenticatedOsuApi,
        CachedAppBaseLayoutMedalDataGenerator appBaseLayoutMedalDataGenerator,
        PermissionService permissionService,
        ITestDataPopulator testDataPopulator)
    {
        _authenticatedOsuApiV2Interface = authenticatedOsuApi;
        _currentSession = currentSession;
        _testDataPopulator = testDataPopulator;
        _staticUrlGenerator = staticUrlGenerator;
        _appBaseLayoutMedalDataGenerator = appBaseLayoutMedalDataGenerator;
        _permissionService = permissionService;
    }
    
    [HttpGet("populateDatabaseWithTestData")]
    public async Task<IActionResult> PopulateDatabaseWithTestData(CancellationToken cancellationToken)
    {
        await _testDataPopulator.PopulateDatabaseAsync(cancellationToken);

        return Ok(new { Message = "The database has been populated with the initial test data" });
    }
    
    [HttpGet("redirectOsuAuth")]
    public IActionResult RedirectOsu(CancellationToken cancellationToken)
    {
        return Redirect(_staticUrlGenerator.Get(StaticUrlGenerator.StaticUrlGeneratorString.OsuLoginString));
    }
    
    [HttpGet("sessionInfo")]
    public async Task<IActionResult> GetSessionInfo(CancellationToken cancellationToken)
    {
        return Ok(new { AccessToken = await _currentSession.GetOsuApiV2TokenAsync(cancellationToken), UserId = await _currentSession.GetOsuApiV2UserIdAsync(cancellationToken) });
    }
        
    [HttpGet("getPermissions")]
    public async Task<IActionResult> GetRoles(CancellationToken cancellationToken)
    {
        if (_currentSession.IsNull())
            throw new NotAuthenticatedException();

        IPermissionStore permissionStore = _currentSession.PermissionStore!;

        return Ok(await permissionStore.GetPermissionsAsync());
    }

    [HttpGet("getMedalsTest")]
    public async Task<IActionResult> MeApiTest(CancellationToken cancellationToken)
    {
        return Ok(await _appBaseLayoutMedalDataGenerator.GenerateAsync(cancellationToken));
    }
    
    [HttpGet("meApiTest")]
    public async Task<IActionResult> GetMedalsTest(CancellationToken cancellationToken)
    {
        return Ok(await _authenticatedOsuApiV2Interface.MeAsync(_currentSession, cancellationToken: cancellationToken));
    }
    
    [HttpGet("userApiTest")]
    public async Task<IActionResult> UserApiTest([FromQuery] string user, CancellationToken cancellationToken)
    {
        return Ok(await _authenticatedOsuApiV2Interface.SearchUserAsync(_currentSession, user, cancellationToken: cancellationToken));
    }
}
#endif