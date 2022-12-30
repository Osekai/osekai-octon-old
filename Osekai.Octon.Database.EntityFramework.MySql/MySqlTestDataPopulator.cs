using Microsoft.EntityFrameworkCore;
using Osekai.Octon.Database.Enums;
using Osekai.Octon.Database.Models;

namespace Osekai.Octon.Database.EntityFramework.MySql;

public class MySqlTestDataPopulator: ITestDataPopulator
{
    private readonly MySqlOsekaiDbContext _context;
    
    public MySqlTestDataPopulator(MySqlOsekaiDbContext context)
    {
        _context = context;
    }
    
    public async Task PopulateDatabaseAsync(CancellationToken cancellationToken = default)
    {
        await _context.Database.ExecuteSqlRawAsync(MySqlDataPopulatorResources.Sql);
        
        await _context.SaveChangesAsync(cancellationToken);
    }
}