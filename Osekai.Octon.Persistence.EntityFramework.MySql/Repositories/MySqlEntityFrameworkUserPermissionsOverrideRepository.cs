using Microsoft.EntityFrameworkCore;
using Osekai.Octon.Persistence.Dtos;
using Osekai.Octon.Persistence.EntityFramework.MySql.Models;
using Osekai.Octon.Persistence.Repositories;

namespace Osekai.Octon.Persistence.EntityFramework.MySql.Repositories;

public class MySqlEntityFrameworkUserPermissionsOverrideRepository: IUserPermissionsOverrideRepository
{
    private MySqlOsekaiDbContext Context { get; }
    
    public MySqlEntityFrameworkUserPermissionsOverrideRepository(MySqlOsekaiDbContext context)
    {
        Context = context;
    }
    
    public async Task<UserPermissionsOverrideDto?> GetUserPermissionOverrideByUserId(int userId, CancellationToken cancellationToken = default)
    {
        UserPermissionsOverride? userPermissionsOverride = await Context.UserPermissionsOverrides.AsNoTracking().FirstOrDefaultAsync(e => e.UserId == userId);
        return userPermissionsOverride?.ToDto();
    }
}