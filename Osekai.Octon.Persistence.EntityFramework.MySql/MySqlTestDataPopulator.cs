using Microsoft.EntityFrameworkCore;
using Osekai.Octon.Persistence.EntityFramework.MySql.Entities;

namespace Osekai.Octon.Persistence.EntityFramework.MySql;

public class MySqlTestDataPopulator: ITestDataPopulator
{
    protected MySqlOsekaiDbContext Context { get; }
    
    public MySqlTestDataPopulator(MySqlOsekaiDbContext context)
    {
        Context = context;
    }
    
    public async Task PopulateDatabaseAsync(CancellationToken cancellationToken = default)
    {
        await Context.Database.EnsureDeletedAsync(cancellationToken);
        await Context.Database.MigrateAsync(cancellationToken);

        await Context.Database.ExecuteSqlRawAsync(MySqlDataPopulatorResources.Sql);
        Context.Locales.Add(new Locale
        {
            Code = "en_GB",
            Flag = "https://assets.ppy.sh/old-flags/GB.png",
            Experimental = false,
            Name = "English (UK)",
            Rtl = false,
            Short = "en",
            Wip = false,
            ExtraCss = null,
            ExtraHtml = null
        });
        
        await Context.SaveChangesAsync(cancellationToken);
    }
}