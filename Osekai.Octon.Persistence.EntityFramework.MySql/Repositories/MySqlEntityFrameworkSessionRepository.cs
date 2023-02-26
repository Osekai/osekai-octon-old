using Microsoft.EntityFrameworkCore;
using Osekai.Octon.Domain.Repositories;
using Osekai.Octon.Domain.ValueObjects;
using Osekai.Octon.Persistence.EntityFramework.MySql.Serializables;
using Session = Osekai.Octon.Domain.AggregateRoots.Session;

namespace Osekai.Octon.Persistence.EntityFramework.MySql.Repositories;

public class MySqlEntityFrameworkSessionRepository: ISessionRepository
{
    private MySqlOsekaiDbContext Context { get; }
    
    public MySqlEntityFrameworkSessionRepository(MySqlOsekaiDbContext context)
    {
        Context = context;
    }

    public async Task<Session?> GetSessionByTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        Entities.Session? session = await Context.Sessions.AsNoTracking().Where(s => s.Token == token && DateTimeOffset.Now < s.ExpiresAt).FirstOrDefaultAsync(cancellationToken);
        if (session == null)
            return null;
        
        Session sessionAggregateRoot = session.ToAggregateRoot();
        
        sessionAggregateRoot.Payload = new SessionPayload(
            session.Payload.OsuApiV2Token, session.Payload.OsuApiV2RefreshToken, 
            session.Payload.OsuUserId, session.Payload.ExpiresAt);

        return sessionAggregateRoot;
    }

    public Task AddSessionAsync(Session session, CancellationToken cancellationToken = default)
    {
        SessionPayload payload = session.Payload?.Value ?? new SessionPayload();
        
        Context.Sessions.Add(new Entities.Session
        {
            Token = session.Token, 
            ExpiresAt = session.ExpiresAt, 
            Payload = new SerializableSessionPayload(payload.OsuApiV2Token, payload.OsuApiV2RefreshToken, payload.OsuUserId, payload.ExpiresAt)
        });
        
        return Task.CompletedTask;
    }

    public async Task<bool> SaveSessionAsyncAsync(Session session, CancellationToken cancellationToken = default)
    {
        Entities.Session? sessionEntity = await Context.Sessions.FindAsync(new object[] { session.Token }, cancellationToken: cancellationToken);
        if (sessionEntity == null)
            return false;

        SessionPayload payload = session.Payload?.Value ?? new SessionPayload();

        sessionEntity.Payload = new SerializableSessionPayload(payload.OsuApiV2Token, payload.OsuApiV2RefreshToken, payload.OsuUserId, payload.ExpiresAt);
        sessionEntity.Token = session.Token;
        sessionEntity.ExpiresAt = session.ExpiresAt;
        
        return true;
    }

    public Task<bool> SessionExistsByToken(string token, CancellationToken cancellationToken = default)
    {
        return Context.Sessions.AsNoTracking().AnyAsync(s => s.Token == token, cancellationToken);
    }

    public async Task DeleteSessionByTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        Entities.Session? sessionEntity = await Context.Sessions.FindAsync(new object[] { token }, cancellationToken: cancellationToken);
        if (sessionEntity != null)
            Context.Remove(sessionEntity);
    }

    public async Task DeleteSessionAsync(Session session, CancellationToken cancellationToken = default)
    {
        Entities.Session? sessionEntity = await Context.Sessions.FindAsync(new object[] { session.Token }, cancellationToken: cancellationToken);
        if (sessionEntity != null)
            Context.Remove(session);
    }
}