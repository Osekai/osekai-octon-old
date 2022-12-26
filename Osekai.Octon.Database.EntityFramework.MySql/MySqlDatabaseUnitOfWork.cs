using Osekai.Octon.Database.EntityFramework.MySql.Repositories;
using Osekai.Octon.Database.Repositories;

namespace Osekai.Octon.Database.EntityFramework.MySql;

public class MySqlDatabaseUnitOfWork: EntityFrameworkDatabaseUnitOfWork<MySqlOsekaiDbContext>
{
    public MySqlDatabaseUnitOfWork(MySqlOsekaiDbContext context) : base(context) { }

    private IAppRepository? _appRepository;
    private ISessionRepository? _sessionRepository;
    private ICacheEntryRepository? _cacheEntryRepository;

    public override IAppRepository AppRepository => 
        _appRepository ??= new MySqlEntityFrameworkAppRepository(Context);

    public override ISessionRepository SessionRepository =>
        _sessionRepository ??= new MySqlEntityFrameworkSessionRepository(Context);

    public override ICacheEntryRepository CacheEntryRepository =>
        _cacheEntryRepository ??= new MySqlEntityFrameworkCacheEntryRepository(Context);
}