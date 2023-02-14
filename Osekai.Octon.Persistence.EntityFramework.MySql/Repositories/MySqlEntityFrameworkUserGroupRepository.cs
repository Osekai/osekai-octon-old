using Microsoft.EntityFrameworkCore;
using Osekai.Octon.Models;
using Osekai.Octon.Persistence.EntityFramework.MySql.Dtos;
using Osekai.Octon.Persistence.EntityFramework.MySql.Entities;
using Osekai.Octon.Persistence.Repositories;

namespace Osekai.Octon.Persistence.EntityFramework.MySql.Repositories;

public class MySqlEntityFrameworkUserGroupRepository: IUserGroupRepository
{
    protected MySqlOsekaiDbContext Context { get; }
    
    public MySqlEntityFrameworkUserGroupRepository(MySqlOsekaiDbContext context)
    {
        Context = context;
    }
    
    public async Task<IReadOnlyUserGroup?> GetUserGroupByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        UserGroup? userGroup = await Context.UserGroups.AsNoTracking().FirstOrDefaultAsync(group => group.Id == id, cancellationToken);
        return userGroup?.ToDto();
    }

    public async Task<IEnumerable<IReadOnlyUserGroup>> GetUserGroupsOfUserAsync(int userId, CancellationToken cancellationToken = default)
    {
        IEnumerable<UserGroup> userGroups = await Context.UserGroupsForUsers
            .AsNoTracking()
            .Include(e => e.UserGroup)
            .Where(e => e.UserId == userId)
            .Select(e => e.UserGroup)
            .OrderBy(e => e.Order)
            .ToArrayAsync(cancellationToken);
        
        return userGroups.Select(e => e.ToDto());
    }

    public async Task<IEnumerable<IReadOnlyUserGroup>> GetUserGroups(CancellationToken cancellationToken = default)
    {
        IEnumerable<UserGroup> userGroups = await Context.UserGroups.ToArrayAsync(cancellationToken);
        return userGroups.Select(e => e.ToDto());
    }
}