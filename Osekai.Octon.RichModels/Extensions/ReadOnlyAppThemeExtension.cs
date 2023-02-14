using Osekai.Octon.Models;

namespace Osekai.Octon.RichModels.Extensions;

internal static class ReadOnlyAppThemeExtension
{
    public static AppTheme ToRichModel(this IReadOnlyAppTheme readOnlyAppTheme)
    {
        return new AppTheme(
            readOnlyAppTheme.Id,
            readOnlyAppTheme.Color,
            readOnlyAppTheme.DarkColor,
            readOnlyAppTheme.HslValueMultiplier,
            readOnlyAppTheme.DarkHslValueMultiplier,
            readOnlyAppTheme.HasCover
        );
    }
}