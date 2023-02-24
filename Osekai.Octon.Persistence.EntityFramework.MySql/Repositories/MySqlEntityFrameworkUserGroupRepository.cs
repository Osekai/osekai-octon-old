using Microsoft.EntityFrameworkCore;
using Osekai.Octon.Domain.AggregateRoots;
using Osekai.Octon.Domain.Repositories;

namespace Osekai.Octon.Persistence.EntityFramework.MySql.Repositories;

public class MySqlEntityFrameworkUserGroupRepository: IUserGroupRepository
{
    protected MySqlOsekaiDbContext Context { get; }
    
    public MySqlEntityFrameworkUserGroupRepository(MySqlOsekaiDbContext context)
    {
        Context = context;
    }
    
    public async Task<UserGroup?> GetUserGroupByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        Entities.UserGroup? userGroup = await Context.UserGroups.AsNoTracking().FirstOrDefaultAsync(group => group.Id == id, cancellationToken);
        return userGroup?.ToAggregateRoot();
    }

    public async Task<IEnumerable<UserGroup>> GetUserGroupsOfUserAsync(int userId, CancellationToken cancellationToken = default)
    {
        IEnumerable<Entities.UserGroup> userGroups = await Context.UserGroupsForUsers
            .AsNoTracking()
            .Include(e => e.UserGroup)
            .Where(e => e.UserId == userId)
            .Select(e => e.UserGroup)
            .OrderBy(e => e.Order)
            .ToArrayAsync(cancellationToken);
        
        return userGroups.Select(e => e.ToAggregateRoot());
    }

    public async Task<IEnumerable<UserGroup>> GetUserGroups(CancellationToken cancellationToken = default)
    {
        IEnumerable<Entities.UserGroup> userGroups = await Context.UserGroups.ToArrayAsync(cancellationToken);
        return userGroups.Select(e => e.ToAggregateRoot());
    }
}