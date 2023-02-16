using Osekai.Octon.Models;
using Osekai.Octon.Persistence.EntityFramework.MySql.Entities;
using Osekai.Octon.Query.QueryResults;

namespace Osekai.Octon.Persistence.EntityFramework.MySql.Dtos;

internal class UserAggregateQueryResultDto: IReadOnlyUserAggregateQueryResult
{
    internal UserAggregateQueryResultDto(IReadOnlyCollection<IReadOnlyUserGroup> userGroups)
    {
        UserGroups = userGroups;
    }

    public IReadOnlyCollection<IReadOnlyUserGroup> UserGroups { get; }
}