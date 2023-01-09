using Osekai.Octon.Database.EntityFramework.MySql.Repositories;
using Osekai.Octon.Database.Repositories;

namespace Osekai.Octon.Database.EntityFramework.MySql;

public class MySqlDatabaseUnitOfWork: EntityFrameworkDatabaseUnitOfWork<MySqlOsekaiDbContext>
{
    public MySqlDatabaseUnitOfWork(MySqlOsekaiDbContext context) : base(context) { }

    private IAppRepository? _appRepository;
    private ISessionRepository? _sessionRepository;
    private IMedalRepository? _medalRepository;
    private ICacheEntryRepository? _cacheEntryRepository;
    private IUserGroupRepository? _userGroupRepository;
    private IUserPermissionsOverrideRepository? _userPermissionsOverrideRepository;

    public override IAppRepository AppRepository => 
        _appRepository ??= new MySqlEntityFrameworkAppRepository(Context);
    
    public override IMedalRepository MedalRepository => 
        _medalRepository ??= new MySqlEntityFrameworkMedalRepository(Context);

    public override IUserGroupRepository UserGroupRepository =>
        _userGroupRepository ??= new MySqlEntityFrameworkUserGroupRepository(Context);

    public override IUserPermissionsOverrideRepository UserPermissionsOverrideRepository =>
        _userPermissionsOverrideRepository ??= new MySqlEntityFrameworkUserPermissionsOverrideRepository(Context);

    public override ISessionRepository SessionRepository =>
        _sessionRepository ??= new MySqlEntityFrameworkSessionRepository(Context);

    public override ICacheEntryRepository CacheEntryRepository =>
        _cacheEntryRepository ??= new MySqlEntityFrameworkCacheEntryRepository(Context);
}