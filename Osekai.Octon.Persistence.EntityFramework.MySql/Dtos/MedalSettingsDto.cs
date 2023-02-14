using Osekai.Octon.Models;

namespace Osekai.Octon.Persistence.EntityFramework.MySql.Dtos;

internal sealed class MedalSettingsDto: IReadOnlyMedalSettings
{
    public MedalSettingsDto(bool locked)
    {
        Locked = locked;
    }
    public bool Locked { get; }
}