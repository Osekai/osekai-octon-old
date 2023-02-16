using Osekai.Octon.Models;

namespace Osekai.Octon.Query.QueryResults;

public interface IReadOnlyAppAggregateQueryResult
{
    public IReadOnlyApp App { get; }
    public IReadOnlyAppTheme? AppTheme { get; }
}