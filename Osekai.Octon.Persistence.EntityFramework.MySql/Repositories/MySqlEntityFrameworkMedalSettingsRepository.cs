using Microsoft.EntityFrameworkCore;
using Osekai.Octon.Models;
using Osekai.Octon.Persistence.EntityFramework.MySql.Dtos;
using Osekai.Octon.Persistence.EntityFramework.MySql.Entities;
using Osekai.Octon.Persistence.Repositories;

namespace Osekai.Octon.Persistence.EntityFramework.MySql.Repositories;

public class MySqlEntityFrameworkMedalSettingsRepository: IMedalSettingsRepository
{
    protected MySqlOsekaiDbContext Context { get; }
    
    public MySqlEntityFrameworkMedalSettingsRepository(MySqlOsekaiDbContext context)
    {
        Context = context;
    }
    
    public async Task<IReadOnlyMedalSettings?> GetMedalSettingsByMedalIdAsync(int medalId, CancellationToken cancellationToken = default)
    {
        MedalSettings? medalSettings = await Context.MedalSettings.Where(e => e.MedalId == medalId).FirstOrDefaultAsync(cancellationToken);
        return medalSettings?.ToDto();
    }
}