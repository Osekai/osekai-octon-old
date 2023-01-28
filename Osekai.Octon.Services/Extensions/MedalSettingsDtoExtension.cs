using Osekai.Octon.Persistence.Dtos;
using Osekai.Octon.Services.Entities;

namespace Osekai.Octon.Services.Extensions;

internal static class MedalSettingsDtoExtension
{
    internal static MedalSettings ToEntity(this MedalSettingsDto medalSettingsDto)
    {
        return new MedalSettings(medalSettingsDto.Locked);
    }
}