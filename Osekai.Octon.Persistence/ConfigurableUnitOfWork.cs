using System.Data;
using Osekai.Octon.Domain.Repositories;

namespace Osekai.Octon.Persistence;

public class ConfigurableUnitOfWork: IUnitOfWork, IDisposable, IAsyncDisposable
{
    public class Builder
    {
        private IAppRepository? _appRepository;
        private ISessionRepository? _sessionRepository;
        private IMedalRepository? _medalRepository;
        private IUserGroupRepository? _userGroupRepository;
        private IUserPermissionsOverrideRepository? _userPermissionsOverrideRepository;
        private IBeatmapPackRepository? _beatmapPackRepository;
        private ILocaleRepository? _localeRepository;
        private ITeamMemberRepository? _teamMemberRepository;

        private readonly List<ISaveStrategy> _saveStrategies;
        
        public Builder()
        {
            _saveStrategies = new List<ISaveStrategy>();
        }

        public Builder WithAppRepository<T>(T appRepository) where T : IAppRepository
        {
            _appRepository = appRepository;
            return this;
        }
        
        public Builder WithSessionRepository<T>(T sessionRepository) where T : ISessionRepository
        {
            _sessionRepository = sessionRepository;
            return this;
        }
        
        public Builder WithMedalRepository<T>(T medalRepository) where T : IMedalRepository
        {
            _medalRepository = medalRepository;
            return this;
        }
        
        public Builder WithUserGroupRepository<T>(T userGroupRepository) where T : IUserGroupRepository
        {
            _userGroupRepository = userGroupRepository;
            return this;
        }
        
        public Builder WithUserPermissionOverrideRepository<T>(T userPermissionsOverrideRepository) where T : IUserPermissionsOverrideRepository
        {
            _userPermissionsOverrideRepository = userPermissionsOverrideRepository;
            return this;
        }
        
        public Builder WithBeatmapPackRepository<T>(T beatmapPackRepository) where T : IBeatmapPackRepository
        {
            _beatmapPackRepository = beatmapPackRepository;
            return this;
        }
        
        public Builder WithLocaleRepository<T>(T localeRepository) where T : ILocaleRepository
        {
            _localeRepository = localeRepository;
            return this;
        }
        
        public Builder WithTeamMemberRepository<T>(T teamMemberRepository) where T : ITeamMemberRepository
        {
            _teamMemberRepository = teamMemberRepository;
            return this;
        }


        public Builder AddSaveStrategy<T>(T saveStrategy) where T : ISaveStrategy
        {
            if (!_saveStrategies.Contains(saveStrategy))
                _saveStrategies.Add((ISaveStrategy)saveStrategy.Clone());

            return this;
        }
        
        public ConfigurableUnitOfWork Build()
        {
            IAppRepository appRepository = _appRepository ?? throw new InvalidOperationException();
            IMedalRepository medalRepository = _medalRepository ?? throw new InvalidOperationException();
            ISessionRepository sessionRepository = _sessionRepository ?? throw new InvalidOperationException();
            IUserGroupRepository userGroupRepository = _userGroupRepository ?? throw new InvalidOperationException();
            IBeatmapPackRepository beatmapPackRepository = _beatmapPackRepository ?? throw new InvalidOperationException();
            IUserPermissionsOverrideRepository userPermissionsOverrideRepository = _userPermissionsOverrideRepository ?? throw new InvalidOperationException();
            ILocaleRepository localeRepository = _localeRepository ?? throw new InvalidOperationException();
            ITeamMemberRepository teamMemberRepository = _teamMemberRepository ?? throw new InvalidOperationException();
            
            return new ConfigurableUnitOfWork(
                appRepository,
                sessionRepository,
                medalRepository,
                userGroupRepository,
                userPermissionsOverrideRepository,
                beatmapPackRepository,
                localeRepository,
                teamMemberRepository,
                _saveStrategies);
        }
    }
    
    private ConfigurableUnitOfWork(
        IAppRepository appRepository, 
        ISessionRepository sessionRepository,
        IMedalRepository medalRepository, 
        IUserGroupRepository userGroupRepository,
        IUserPermissionsOverrideRepository userPermissionsOverrideRepository,
        IBeatmapPackRepository beatmapPackRepository,
        ILocaleRepository localeRepository,
        ITeamMemberRepository teamMemberRepository,
        IEnumerable<ISaveStrategy> saveStrategies)
    {
        AppRepository = appRepository;
        SessionRepository = sessionRepository;
        MedalRepository = medalRepository;
        UserGroupRepository = userGroupRepository;
        UserPermissionsOverrideRepository = userPermissionsOverrideRepository;
        BeatmapPackRepository = beatmapPackRepository;
        LocaleRepository = localeRepository;
        TeamMemberRepository = teamMemberRepository;
        _saveStrategies = saveStrategies.ToList();
    }
    
    private readonly IReadOnlyList<ISaveStrategy> _saveStrategies;

    private volatile bool _disposed;
    
    public async ValueTask DisposeAsync()
    {
        if (_disposed)
            return;

        _disposed = true;

        foreach (var saveStrategy in _saveStrategies)
            await saveStrategy.DisposeAsync();
    }

    public void Dispose()
    {
        if (_disposed)
            return;

        _disposed = true;

        foreach (var saveStrategy in _saveStrategies)
            saveStrategy.Dispose();
    }

    public IAppRepository AppRepository { get; }
    public ISessionRepository SessionRepository { get; }
    public IMedalRepository MedalRepository { get; }
    public IUserGroupRepository UserGroupRepository { get; }
    public IUserPermissionsOverrideRepository UserPermissionsOverrideRepository { get; }
    public IBeatmapPackRepository BeatmapPackRepository { get; }
    public ILocaleRepository LocaleRepository { get; }
    public ITeamMemberRepository TeamMemberRepository { get; }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var saveStrategy in _saveStrategies)
            await saveStrategy.SaveAsync(cancellationToken);
    }
}