using Microsoft.EntityFrameworkCore;
using Osekai.Octon.Database.Dtos;
using Osekai.Octon.Database.EntityFramework.MySql.Models;
using Osekai.Octon.Database.Repositories;

namespace Osekai.Octon.Database.EntityFramework.MySql.Repositories;

public class MySqlEntityFrameworkUserGroupRepository: IUserGroupRepository
{
    private readonly MySqlOsekaiDbContext _context;
    
    public MySqlEntityFrameworkUserGroupRepository(MySqlOsekaiDbContext context)
    {
        _context = context;
    }
    
    public async Task<UserGroupDto?> GetUserGroupByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        UserGroup? userGroup = await _context.UserGroups.FirstOrDefaultAsync(group => group.Id == id);
        return userGroup?.ToDto();
    }

    public async Task<IEnumerable<UserGroupDto>> GetUserGroupOfUserAsync(int userId, CancellationToken cancellationToken = default)
    {
        IEnumerable<UserGroup> userGroups = await _context.UserGroupsForUsers
            .Include(e => e.UserGroup)
            .Where(e => e.UserId == userId)
            .Select(e => e.UserGroup).ToArrayAsync();
        
        return userGroups.Select(e => e.ToDto());
    }
}