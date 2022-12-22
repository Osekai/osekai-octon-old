using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Osekai.Octon.Database;

namespace Osekai.Octon.API;

#if DEBUG
[Route("/api/dev")]
public class DevController: Controller
{
    private readonly ITestDataPopulator _testDataPopulator;
    private readonly DbContext _dbContext;
    
    public DevController(DbContext dbContext, ITestDataPopulator testDataPopulator)
    {
        _testDataPopulator = testDataPopulator;
        _dbContext = dbContext;
    }
    
    [HttpGet("populateDatabaseWithTestData")]
    public async Task<IActionResult> PopulateDatabaseWithTestData(CancellationToken cancellationToken)
    {
        await _dbContext.Database.EnsureDeletedAsync(cancellationToken);
        await _dbContext.Database.MigrateAsync(cancellationToken);
        
        await _testDataPopulator.PopulateDatabaseAsync(cancellationToken);

        return Ok(new { Message = "The database has been populated with the initial test data" });
    }
}
#endif