using Microsoft.EntityFrameworkCore;
using Osekai.Octon.Domain.Repositories;

namespace Osekai.Octon.Persistence.EntityFramework.MySql.Repositories;

public class MySqlEntityFrameworkUserPermissionsOverrideRepository: IUserPermissionsOverrideRepository
{
    private MySqlOsekaiDbContext Context { get; }
    
    public MySqlEntityFrameworkUserPermissionsOverrideRepository(MySqlOsekaiDbContext context)
    {
        Context = context;
    }
    
    public async Task<Domain.AggregateRoots.UserPermissionsOverride?> GetUserPermissionOverrideByUserId(int userId, CancellationToken cancellationToken = default)
    {
        Entities.UserPermissionsOverride? userPermissionsOverride = await Context.UserPermissionsOverrides.AsNoTracking().FirstOrDefaultAsync(e => e.UserId == userId);
        return userPermissionsOverride?.ToEntity();
    }
}