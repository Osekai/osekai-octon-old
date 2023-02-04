using Osekai.Octon.Persistence.EntityFramework.MySql.Repositories;
using Osekai.Octon.Persistence.Repositories;

namespace Osekai.Octon.Persistence.EntityFramework.MySql;

public class MySqlUnitOfWork: EntityFrameworkUnitOfWork<MySqlOsekaiDbContext>
{
    public MySqlUnitOfWork(MySqlOsekaiDbContext context) : base(context) { }

    private IAppRepository? _appRepository;
    private ISessionRepository? _sessionRepository;
    private IMedalRepository? _medalRepository;
    private IUserGroupRepository? _userGroupRepository;
    private IUserPermissionsOverrideRepository? _userPermissionsOverrideRepository;
    private IAppThemeRepository? _appThemeRepository;
    private IMedalSettingsRepository? _medalSettingsRepository;
    private IBeatmapPackRepository? _beatmapPackRepository;
    private IMedalSolutionRepository? _medalSolutionRepository;

    public override IAppRepository AppRepository => 
        _appRepository ??= new MySqlEntityFrameworkAppRepository(Context);
    
    public override IMedalRepository MedalRepository => 
        _medalRepository ??= new MySqlEntityFrameworkMedalRepository(Context);

    public override IUserGroupRepository UserGroupRepository =>
        _userGroupRepository ??= new MySqlEntityFrameworkUserGroupRepository(Context);

    public override IAppThemeRepository AppThemeRepository =>
        _appThemeRepository ??= new MySqlEntityFrameworkAppThemeRepository(Context);

    public override IUserPermissionsOverrideRepository UserPermissionsOverrideRepository =>
        _userPermissionsOverrideRepository ??= new MySqlEntityFrameworkUserPermissionsOverrideRepository(Context);

    public override IMedalSettingsRepository MedalSettingsRepository =>
        _medalSettingsRepository ??= new MySqlEntityFrameworkMedalSettingsRepository(Context);

    public override IMedalSolutionRepository MedalSolutionRepository =>
        _medalSolutionRepository ??= new MySqlEntityFrameworkMedalSolutionRepository(Context);

    public override IBeatmapPackRepository BeatmapPackRepository =>
        _beatmapPackRepository ??= new MySqlEntityFrameworkBeatmapPackRepository(Context);

    public override ISessionRepository SessionRepository =>
        _sessionRepository ??= new MySqlEntityFrameworkSessionRepository(Context);
}