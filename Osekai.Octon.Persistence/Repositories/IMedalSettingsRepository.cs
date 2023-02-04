using Osekai.Octon.Objects;

namespace Osekai.Octon.Persistence.Repositories;

public interface IMedalSettingsRepository
{
    Task<IReadOnlyMedalSettings?> GetMedalSettingsByMedalIdAsync(int medalId, CancellationToken cancellationToken = default);
}