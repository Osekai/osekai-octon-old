using Osekai.Octon.Objects;
using Osekai.Octon.Services.Entities;

namespace Osekai.Octon.Services.Extensions;

internal static class ReadOnlyMedalSettingsExtension
{
    internal static MedalSettings ToEntity(this IReadOnlyMedalSettings medalSettingsDto)
    {
        return new MedalSettings(medalSettingsDto.Locked);
    }
}