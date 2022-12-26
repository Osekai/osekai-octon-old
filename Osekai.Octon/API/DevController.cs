using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Osekai.Octon.Applications;
using Osekai.Octon.Applications.OsuApi;
using Osekai.Octon.Database;

namespace Osekai.Octon.API;

#if DEBUG
[Route("/api/dev")]
[DefaultAuthenticationFilter]
public class DevController: Controller
{
    private readonly ITestDataPopulator _testDataPopulator;
    private readonly DbContext _dbContext;
    private readonly StaticUrlGenerator _staticUrlGenerator;
    private readonly CurrentSession _currentSession;
    private readonly CachedAuthenticatedOsuApiV2Interface _authenticatedOsuApiV2Interface;
    
    public DevController(DbContext dbContext, 
        StaticUrlGenerator staticUrlGenerator,
        CurrentSession currentSession,
        CachedAuthenticatedOsuApiV2Interface authenticatedOsuApi,
        ITestDataPopulator testDataPopulator)
    {
        _authenticatedOsuApiV2Interface = authenticatedOsuApi;
        _currentSession = currentSession;
        _testDataPopulator = testDataPopulator;
        _dbContext = dbContext;
        _staticUrlGenerator = staticUrlGenerator;
    }
    
    [HttpGet("populateDatabaseWithTestData")]
    public async Task<IActionResult> PopulateDatabaseWithTestData(CancellationToken cancellationToken)
    {
        await _dbContext.Database.EnsureDeletedAsync(cancellationToken);
        await _dbContext.Database.MigrateAsync(cancellationToken);
        
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
        return Ok(await _currentSession.GetAsync(cancellationToken));
    }
    
    [HttpGet("meApiTest")]
    public async Task<IActionResult> MeApiTest(CancellationToken cancellationToken)
    {
        return Ok(await _authenticatedOsuApiV2Interface.MeAsync(cancellationToken: cancellationToken));
    }
    
    [HttpGet("userApiTest")]
    public async Task<IActionResult> UserApiTest([FromQuery] string user, CancellationToken cancellationToken)
    {
        return Ok(await _authenticatedOsuApiV2Interface.SearchUserAsync(user, cancellationToken: cancellationToken));
    }
}
#endif