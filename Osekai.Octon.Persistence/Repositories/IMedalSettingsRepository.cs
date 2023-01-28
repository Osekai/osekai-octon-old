using Osekai.Octon.Persistence.Dtos;

namespace Osekai.Octon.Persistence.Repositories;

public interface IMedalSettingsRepository
{
    Task<MedalSettingsDto?> GetMedalSettingsByMedalIdAsync(int medalId, CancellationToken cancellationToken = default);
}