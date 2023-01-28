using Osekai.Octon.Persistence.Dtos;
using Osekai.Octon.Services.Entities;

namespace Osekai.Octon.Services.Extensions;

internal static class AppThemeDtoExtension
{
    internal static AppTheme ToEntity(this AppThemeDto appThemeDto)
    {
        return new AppTheme(
            appThemeDto.Id,
            appThemeDto.Color,
            appThemeDto.DarkColor,
            appThemeDto.HslValueMultiplier,
            appThemeDto.DarkHslValueMultiplier,
            appThemeDto.HasCover
        );
    }
}