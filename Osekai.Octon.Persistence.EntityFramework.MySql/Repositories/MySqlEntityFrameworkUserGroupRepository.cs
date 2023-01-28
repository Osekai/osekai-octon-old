using Microsoft.EntityFrameworkCore;
using Osekai.Octon.Persistence.Dtos;
using Osekai.Octon.Persistence.EntityFramework.MySql.Models;
using Osekai.Octon.Persistence.Repositories;

namespace Osekai.Octon.Persistence.EntityFramework.MySql.Repositories;

public class MySqlEntityFrameworkUserGroupRepository: IUserGroupRepository
{
    protected MySqlOsekaiDbContext Context { get; }
    
    public MySqlEntityFrameworkUserGroupRepository(MySqlOsekaiDbContext context)
    {
        Context = context;
    }
    
    public async Task<UserGroupDto?> GetUserGroupByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        UserGroup? userGroup = await Context.UserGroups.AsNoTracking().FirstOrDefaultAsync(group => group.Id == id);
        return userGroup?.ToDto();
    }

    public async Task<IEnumerable<UserGroupDto>> GetUserGroupsOfUserAsync(int userId, CancellationToken cancellationToken = default)
    {
        IEnumerable<UserGroup> userGroups = await Context.UserGroupsForUsers
            .AsNoTracking()
            .Include(e => e.UserGroup)
            .Where(e => e.UserId == userId)
            .Select(e => e.UserGroup)
            .OrderBy(e => e.Order)
            .ToArrayAsync();
        
        return userGroups.Select(e => e.ToDto());
    }
}