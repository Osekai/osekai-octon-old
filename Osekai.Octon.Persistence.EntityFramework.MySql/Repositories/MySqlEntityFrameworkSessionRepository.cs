using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Osekai.Octon.Domain.Entities;
using Osekai.Octon.Domain.Repositories;
using Session = Osekai.Octon.Domain.Aggregates.Session;

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
        return session?.ToAggregate();
    }

    public Task AddSessionAsync(Session session, CancellationToken cancellationToken = default)
    {
        Context.Sessions.Add(new Entities.Session
        {
            Token = session.Token, 
            ExpiresAt = session.ExpiresAt, 
            Payload = JsonSerializer.Serialize(session.Payload)
        });
        
        return Task.CompletedTask;
    }

    public async Task<bool> SaveSessionAsyncAsync(Session session, CancellationToken cancellationToken = default)
    {
        Entities.Session? sessionEntity = await Context.Sessions.FindAsync(new object[] { session.Token }, cancellationToken: cancellationToken);
        if (sessionEntity == null)
            return false;

        sessionEntity.Payload = JsonSerializer.Serialize(session.Payload);
        sessionEntity.Token = session.Token;
        sessionEntity.ExpiresAt = session.ExpiresAt;
        
        return true;
    }

    public Task<Session> AddSessionAsync(string token, SessionPayload payload, DateTimeOffset expiresAt, CancellationToken cancellationToken = default)
    {
        Context.Sessions.Add(new Entities.Session
        {
            Token = token, 
            ExpiresAt = expiresAt, 
            Payload = JsonSerializer.Serialize(payload)
        });
        
        return Task.FromResult<Session>(new Session(token, payload, expiresAt));
    }

    public async Task<bool> UpdateSessionPayloadAsync(
        string token, SessionPayload payload,
        CancellationToken cancellationToken = default)
    {
        Entities.Session? session = await Context.Sessions.FindAsync(new object[] { token }, cancellationToken: cancellationToken);
        if (session == null)
            return false;

        session.Payload = JsonSerializer.Serialize(payload);
        return true;
    }

    public async Task<bool> UpdateExpirationDateTimeAsync(string token, DateTimeOffset dateTime,
        CancellationToken cancellationToken = default)
    {
        Entities.Session? session = await Context.Sessions.FindAsync(new object[] { token }, cancellationToken: cancellationToken);
        if (session == null)
            return false;

        session.ExpiresAt = dateTime;
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