using Microsoft.EntityFrameworkCore;
using Osekai.Octon.Persistence.EntityFramework.MySql.Dtos;
using Osekai.Octon.Persistence.EntityFramework.MySql.Entities;
using Osekai.Octon.Query;
using Osekai.Octon.Query.QueryParams;
using Osekai.Octon.Query.QueryResults;

namespace Osekai.Octon.Persistence.EntityFramework.MySql;

public class MySqlEntityFrameworkUserAggregateQuery: IParameterizedQuery<IReadOnlyUserAggregateQueryResult, UserAggregateParam>
{
    private readonly MySqlOsekaiDbContext _context;
    
    public MySqlEntityFrameworkUserAggregateQuery(MySqlOsekaiDbContext context)
    {
        _context = context;
    }
    
    public async Task<IReadOnlyUserAggregateQueryResult> ExecuteAsync(UserAggregateParam param, CancellationToken cancellationToken)
    {
        IEnumerable<UserGroup> userGroups = await _context.UserGroupsForUsers.Where(e => e.UserId == param.UserId).Select(e => e.UserGroup).ToArrayAsync(cancellationToken);
        return new UserAggregateQueryResultDto(userGroups.Select(e => e.ToDto()).ToArray());
    }
}