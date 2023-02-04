using Osekai.Octon.Objects;
using Osekai.Octon.Objects.Aggregators;

namespace Osekai.Octon.Persistence.EntityFramework.MySql.Dtos;

internal class AppWithAppThemeDto: IReadOnlyAppWithAppTheme
{
    public IReadOnlyApp App { get; init; }
    public IReadOnlyAppTheme? AppTheme { get; init; }
}