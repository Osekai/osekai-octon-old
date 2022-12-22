using Osekai.Octon.Database.Models;

namespace Osekai.Octon.Database.EntityFramework;

public class MySqlTestDataPopulator: ITestDataPopulator
{
    private MySqlOsekaiDbContext _context;
    
    public MySqlTestDataPopulator(MySqlOsekaiDbContext context)
    {
        _context = context;
    }
    
    public async Task PopulateDatabaseAsync(CancellationToken cancellationToken = default)
    {
        _context.Apps.Add(
            new App
            {
                Name = "Home",
                Id = -1,
                Order = 1,
                Experimental = false,
                Visible = true,
                SimpleName = "home",
                AppTheme = new AppTheme
                {
                    Color = "53,61,85",
                    DarkColor = "53,61,85",
                    HasCover = true,
                    HslValueMultiplier = 1,
                    DarkHslValueMultiplier = 1
                }
            });

        await _context.SaveChangesAsync(cancellationToken);
    }
}