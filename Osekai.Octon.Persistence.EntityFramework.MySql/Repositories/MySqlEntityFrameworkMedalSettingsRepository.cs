using Microsoft.EntityFrameworkCore;
using Osekai.Octon.Persistence.Dtos;
using Osekai.Octon.Persistence.EntityFramework.MySql.Models;
using Osekai.Octon.Persistence.Repositories;

namespace Osekai.Octon.Persistence.EntityFramework.MySql.Repositories;

public class MySqlEntityFrameworkMedalSettingsRepository: IMedalSettingsRepository
{
    protected MySqlOsekaiDbContext Context { get; }
    
    public MySqlEntityFrameworkMedalSettingsRepository(MySqlOsekaiDbContext context)
    {
        Context = context;
    }
    
    public async Task<MedalSettingsDto?> GetMedalSettingsByMedalIdAsync(int medalId, CancellationToken cancellationToken = default)
    {
        MedalSettings? medalSettings = await Context.MedalSettings.Where(e => e.MedalId == medalId).FirstOrDefaultAsync(cancellationToken);
        return medalSettings?.ToDto();
    }
}