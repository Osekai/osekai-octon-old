using Osekai.Octon.Models;

namespace Osekai.Octon.Query.QueryResults;

public interface IReadOnlyUserAggregateQueryResult
{
    IReadOnlyCollection<IReadOnlyUserGroup> UserGroups { get; }
}