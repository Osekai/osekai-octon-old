using Osekai.Octon.Models;
using Osekai.Octon.Query.QueryResults;

namespace Osekai.Octon.Persistence.EntityFramework.MySql.Dtos;

internal class AppAggregateQueryResultDto: IReadOnlyAppAggregateQueryResult
{
    public IReadOnlyApp App { get; init; } = null!;
    public IReadOnlyAppTheme? AppTheme { get; init; }
}