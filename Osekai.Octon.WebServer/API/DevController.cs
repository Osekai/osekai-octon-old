using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Osekai.Octon;
using Osekai.Octon.DataAdapter;
using Osekai.Octon.OsuApi;
using Osekai.Octon.Database;
using Osekai.Octon.Exceptions;
using Osekai.Octon.Services;

namespace Osekai.Octon.WebServer.API;

#if DEBUG
[Route("/api/dev")]
[DefaultAuthenticationFilter]
public class DevController: Controller
{
    private readonly ITestDataPopulator _testDataPopulator;
    private readonly IDatabaseUnitOfWorkFactory _databaseUnitOfWorkFactory;
    private readonly StaticUrlGenerator _staticUrlGenerator;
    private readonly CurrentSession _currentSession;
    private readonly CachedAuthenticatedOsuApiV2Interface _authenticatedOsuApiV2Interface;
    private readonly CachedOsekaiDataAdapter _osekaiDataAdapter;
    private readonly PermissionService _permissionService;
    
    public DevController(StaticUrlGenerator staticUrlGenerator,
        CurrentSession currentSession,
        CachedAuthenticatedOsuApiV2Interface authenticatedOsuApi,
        IDatabaseUnitOfWorkFactory databaseUnitOfWorkFactory,
        CachedOsekaiDataAdapter osekaiDataAdapter,
        PermissionService permissionService,
        ITestDataPopulator testDataPopulator)
    {
        _authenticatedOsuApiV2Interface = authenticatedOsuApi;
        _currentSession = currentSession;
        _databaseUnitOfWorkFactory = databaseUnitOfWorkFactory;
        _testDataPopulator = testDataPopulator;
        _staticUrlGenerator = staticUrlGenerator;
        _osekaiDataAdapter = osekaiDataAdapter;
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
        
    [HttpGet("getRoles")]
    public async Task<IActionResult> GetRoles(CancellationToken cancellationToken)
    {
        if (_currentSession.IsNull())
            throw new NotAuthenticatedException();
        
        int userId = (await _currentSession.GetOsuApiV2UserIdAsync(cancellationToken))!.Value;
        return Ok(new { Permissions = await _permissionService.GetUserPermissionsAsync(userId) });
    }

    [HttpGet("getMedalsTest")]
    public async Task<IActionResult> MeApiTest(CancellationToken cancellationToken)
    {
        return Ok(await _osekaiDataAdapter.GetMedalDataAsync(cancellationToken));
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