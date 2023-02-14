using Osekai.Octon.Models;
using Osekai.Octon.Persistence.QueryResults;

namespace Osekai.Octon.Persistence.EntityFramework.MySql.Dtos;

internal class AppAggregateQueryResultDto: IReadOnlyAppAggregateQueryResult
{
    public IReadOnlyApp App { get; init; }
    public IReadOnlyAppTheme? AppTheme { get; init; }
}