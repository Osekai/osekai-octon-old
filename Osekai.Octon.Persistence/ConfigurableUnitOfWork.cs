﻿using System.Data;
using Osekai.Octon.Persistence.Repositories;

namespace Osekai.Octon.Persistence;

public class ConfigurableUnitOfWork: IUnitOfWork, IDisposable, IAsyncDisposable
{
    public class Builder
    {
        private IAppRepository? _appRepository;
        private ISessionRepository? _sessionRepository;
        private IMedalRepository? _medalRepository;
        private IUserGroupRepository? _userGroupRepository;
        private IAppThemeRepository? _appThemeRepository;
        private IUserPermissionsOverrideRepository? _userPermissionsOverrideRepository;
        private IMedalSettingsRepository? _medalSettingsRepository;
        private IMedalSolutionRepository? _medalSolutionRepository;
        private IBeatmapPackRepository? _beatmapPackRepository;
        private ILocaleRepository? _localeRepository;

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
        
        public Builder WithAppThemeRepository<T>(T appThemeRepository) where T : IAppThemeRepository
        {
            _appThemeRepository = appThemeRepository;
            return this;
        }
        
        public Builder WithUserPermissionOverrideRepository<T>(T userPermissionsOverrideRepository) where T : IUserPermissionsOverrideRepository
        {
            _userPermissionsOverrideRepository = userPermissionsOverrideRepository;
            return this;
        }
        
        public Builder WithMedalSettings<T>(T medalSettingsRepository) where T : IMedalSettingsRepository
        {
            _medalSettingsRepository = medalSettingsRepository;
            return this;
        }
        
        public Builder WithMedalSolution<T>(T medalSolutionRepository) where T : IMedalSolutionRepository
        {
            _medalSolutionRepository = medalSolutionRepository;
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

        public Builder AddSaveStrategy<T>(T saveStrategy) where T : ISaveStrategy
        {
            if (!_saveStrategies.Contains(saveStrategy))
                _saveStrategies.Add((ISaveStrategy)saveStrategy.Clone());

            return this;
        }
        
        public ConfigurableUnitOfWork Build()
        {
            IAppRepository appRepository = _appRepository ?? throw new InvalidOperationException();
            IAppThemeRepository appThemeRepository = _appThemeRepository ?? throw new InvalidOperationException();
            IMedalRepository medalRepository = _medalRepository ?? throw new InvalidOperationException();
            IMedalSettingsRepository medalSettingsRepository = _medalSettingsRepository ?? throw new InvalidOperationException();
            IMedalSolutionRepository medalSolutionRepository = _medalSolutionRepository ?? throw new InvalidOperationException();
            ISessionRepository sessionRepository = _sessionRepository ?? throw new InvalidOperationException();
            IUserGroupRepository userGroupRepository = _userGroupRepository ?? throw new InvalidOperationException();
            IBeatmapPackRepository beatmapPackRepository = _beatmapPackRepository ?? throw new InvalidOperationException();
            IUserPermissionsOverrideRepository userPermissionsOverrideRepository = _userPermissionsOverrideRepository ?? throw new InvalidOperationException();
            ILocaleRepository localeRepository = _localeRepository ?? throw new InvalidOperationException();

            return new ConfigurableUnitOfWork(
                appRepository,
                sessionRepository,
                medalRepository,
                userGroupRepository,
                appThemeRepository,
                userPermissionsOverrideRepository,
                medalSettingsRepository,
                medalSolutionRepository,
                beatmapPackRepository,
                _saveStrategies,
                localeRepository);
        }
    }
    
    private ConfigurableUnitOfWork(
        IAppRepository appRepository, 
        ISessionRepository sessionRepository,
        IMedalRepository medalRepository, 
        IUserGroupRepository userGroupRepository,
        IAppThemeRepository appThemeRepository,
        IUserPermissionsOverrideRepository userPermissionsOverrideRepository,
        IMedalSettingsRepository medalSettingsRepository, 
        IMedalSolutionRepository medalSolutionRepository, 
        IBeatmapPackRepository beatmapPackRepository,
        IEnumerable<ISaveStrategy> saveStrategies, ILocaleRepository localeRepository)
    {
        AppRepository = appRepository;
        SessionRepository = sessionRepository;
        MedalRepository = medalRepository;
        UserGroupRepository = userGroupRepository;
        AppThemeRepository = appThemeRepository;
        UserPermissionsOverrideRepository = userPermissionsOverrideRepository;
        MedalSettingsRepository = medalSettingsRepository;
        MedalSolutionRepository = medalSolutionRepository;
        BeatmapPackRepository = beatmapPackRepository;
        LocaleRepository = localeRepository;
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
    public IAppThemeRepository AppThemeRepository { get; } 
    public IUserPermissionsOverrideRepository UserPermissionsOverrideRepository { get; }
    public IMedalSettingsRepository MedalSettingsRepository { get; }
    public IMedalSolutionRepository MedalSolutionRepository { get; } 
    public IBeatmapPackRepository BeatmapPackRepository { get; }
    public ILocaleRepository LocaleRepository { get; }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var saveStrategy in _saveStrategies)
            await saveStrategy.SaveAsync(cancellationToken);
    }
}