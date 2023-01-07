using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Osekai.Octon;
using Osekai.Octon.DataAdapter;
using Osekai.Octon.OsuApi;
using Osekai.Octon.Database;

namespace Osekai.Octon.WebServer.API;

#if DEBUG
[Route("/api/dev")]
[DefaultAuthenticationFilter]
public class DevController: Controller
{
    private readonly ITestDataPopulator _testDataPopulator;
    private readonly DbContext _dbContext;
    private readonly IDatabaseUnitOfWorkFactory _databaseUnitOfWorkFactory;
    private readonly StaticUrlGenerator _staticUrlGenerator;
    private readonly CurrentSession _currentSession;
    private readonly CachedAuthenticatedOsuApiV2Interface _authenticatedOsuApiV2Interface;
    private readonly CachedOsekaiDataAdapter _osekaiDataAdapter;
    
    public DevController(DbContext dbContext, 
        StaticUrlGenerator staticUrlGenerator,
        CurrentSession currentSession,
        CachedAuthenticatedOsuApiV2Interface authenticatedOsuApi,
        IDatabaseUnitOfWorkFactory databaseUnitOfWorkFactory,
        CachedOsekaiDataAdapter osekaiDataAdapter,
        ITestDataPopulator testDataPopulator)
    {
        _authenticatedOsuApiV2Interface = authenticatedOsuApi;
        _currentSession = currentSession;
        _databaseUnitOfWorkFactory = databaseUnitOfWorkFactory;
        _testDataPopulator = testDataPopulator;
        _dbContext = dbContext;
        _staticUrlGenerator = staticUrlGenerator;
        _osekaiDataAdapter = osekaiDataAdapter;
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
        return Ok(new { AccessToken = await _currentSession.GetOsuApiV2TokenAsync(cancellationToken), UserId = await _currentSession.GetOsuApiV2UserIdAsync(cancellationToken) });
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