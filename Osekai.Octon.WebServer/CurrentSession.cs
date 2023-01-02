using Osekai.Octon.Database.Dtos;
using Osekai.Octon.Database.HelperTypes;
using Osekai.Octon.Exceptions;
using Osekai.Octon.OsuApi;
using Osekai.Octon.Services;

namespace Osekai.Octon.WebServer;

public class CurrentSession: IOsuApiV2SessionProvider
{
    private SessionDto? _session;
    private readonly SessionService _sessionService;

    private readonly ILogger<CurrentSession> _logger;

    public CurrentSession(
        SessionService sessionService,
        ILogger<CurrentSession> logger)
    {
        _sessionService = sessionService;
        _logger = logger;
    }
    
    public void Set(SessionDto session)
    {
        _session = session;
    }

    private async Task UpdateIfNeededAsync(CancellationToken cancellationToken)
    {
        if (_session == null)
            return;
        
        try
        {
            if (DateTimeOffset.Now > _session.ExpiresAt)
                throw new InvalidSessionTokenException(_session.Token);

            if (DateTimeOffset.UtcNow > _session.Payload.ExpiresAt)
            {
                SessionDtoPayload payload = await _sessionService.RefreshOsuSessionAsync(_session.Token, cancellationToken);
                _session = new SessionDto(_session.Token, payload, _session.ExpiresAt);
            }
        }
        catch
        {
            _session = null;
            
            throw;
        }
    }
    
    public async Task<SessionDto?> GetAsync(CancellationToken cancellationToken = default)
    {
        if (_session == null)
            return null;

        await UpdateIfNeededAsync(cancellationToken);

        return (SessionDto)_session!.Clone();
    }

    public async Task<bool> TryUpdateAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await UpdateIfNeededAsync(cancellationToken);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool IsNull() => _session == null;
    
    public async Task<string?> GetOsuApiV2TokenAsync(CancellationToken cancellationToken = default)
    {
        await UpdateIfNeededAsync(cancellationToken);
        return _session?.Payload.OsuApiV2Token;
    }

    public Task<int?> GetOsuApiV2UserIdAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(OsuUserId);
    }

    public int? OsuUserId => _session?.Payload.OsuUserId;
}