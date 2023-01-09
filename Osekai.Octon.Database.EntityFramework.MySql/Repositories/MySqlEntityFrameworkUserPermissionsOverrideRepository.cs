using Microsoft.EntityFrameworkCore;
using Osekai.Octon.Database.Dtos;
using Osekai.Octon.Database.EntityFramework.MySql.Models;
using Osekai.Octon.Database.Repositories;

namespace Osekai.Octon.Database.EntityFramework.MySql.Repositories;

public class MySqlEntityFrameworkUserPermissionsOverrideRepository: IUserPermissionsOverrideRepository
{
    private readonly MySqlOsekaiDbContext _context;
    
    public MySqlEntityFrameworkUserPermissionsOverrideRepository(MySqlOsekaiDbContext context)
    {
        _context = context;
    }
    
    public async Task<UserPermissionsOverrideDto?> GetUserPermissionOverrideByUserId(int userId, CancellationToken cancellationToken = default)
    {
        UserPermissionsOverride? userPermissionsOverride = await _context.UserPermissionsOverrides.FirstOrDefaultAsync(e => e.UserId == userId);
        return userPermissionsOverride?.ToDto();
    }
}