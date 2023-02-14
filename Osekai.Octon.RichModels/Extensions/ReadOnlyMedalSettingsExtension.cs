using Osekai.Octon.Models;

namespace Osekai.Octon.RichModels.Extensions;

internal static class ReadOnlyMedalSettingsExtension
{
    public static MedalSettings ToRichModel(this IReadOnlyMedalSettings medalSettingsDto)
    {
        return new MedalSettings(medalSettingsDto.Locked);
    }
}