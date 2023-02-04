using Osekai.Octon.Objects;
using Osekai.Octon.Services.Entities;

namespace Osekai.Octon.Services.Extensions;

internal static class AppThemeDtoExtension
{
    internal static AppTheme ToEntity(this IReadOnlyAppTheme appTheme)
    {
        return new AppTheme(
            appTheme.Id,
            appTheme.Color,
            appTheme.DarkColor,
            appTheme.HslValueMultiplier,
            appTheme.DarkHslValueMultiplier,
            appTheme.HasCover
        );
    }
}