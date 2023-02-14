using Osekai.Octon.Models;

namespace Osekai.Octon.Persistence.QueryResults;

public interface IReadOnlyAppAggregateQueryResult
{
    public IReadOnlyApp App { get; }
    public IReadOnlyAppTheme? AppTheme { get; }
}