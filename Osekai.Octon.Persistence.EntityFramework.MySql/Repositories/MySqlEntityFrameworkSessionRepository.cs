using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Osekai.Octon.HelperTypes;
using Osekai.Octon.Models;
using Osekai.Octon.Persistence.EntityFramework.MySql.Dtos;
using Osekai.Octon.Persistence.EntityFramework.MySql.Entities;
using Osekai.Octon.Persistence.Repositories;

namespace Osekai.Octon.Persistence.EntityFramework.MySql.Repositories;

public class MySqlEntityFrameworkSessionRepository: ISessionRepository
{
    private MySqlOsekaiDbContext Context { get; }
    
    public MySqlEntityFrameworkSessionRepository(MySqlOsekaiDbContext context)
    {
        Context = context;
    }

    public async Task<IReadOnlySession?> GetSessionByTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        Session? session = await Context.Sessions.AsNoTracking().Where(s => s.Token == token && DateTimeOffset.Now < s.ExpiresAt).FirstOrDefaultAsync(cancellationToken);
        return session?.ToDto();
    }

    public Task AddSessionAsync(IReadOnlySession session, CancellationToken cancellationToken = default)
    {
        Context.Sessions.Add(new Session
        {
            Token = session.Token, 
            ExpiresAt = session.ExpiresAt, 
            Payload = JsonSerializer.Serialize(session.Payload)
        });
        
        return Task.CompletedTask;
    }

    public async Task<bool> SaveSessionAsyncAsync(IReadOnlySession session, CancellationToken cancellationToken = default)
    {
        Session? sessionEntity = await Context.Sessions.FindAsync(new object[] { session.Token }, cancellationToken: cancellationToken);
        if (sessionEntity == null)
            return false;

        sessionEntity.Payload = JsonSerializer.Serialize(session.Payload);
        sessionEntity.Token = session.Token;
        sessionEntity.ExpiresAt = session.ExpiresAt;
        
        return true;
    }

    public Task<IReadOnlySession> AddSessionAsync(string token, SessionPayload payload, DateTimeOffset expiresAt, CancellationToken cancellationToken = default)
    {
        Context.Sessions.Add(new Session
        {
            Token = token, 
            ExpiresAt = expiresAt, 
            Payload = JsonSerializer.Serialize(payload)
        });
        
        return Task.FromResult<IReadOnlySession>(new SessionDto(token, payload, expiresAt));
    }

    public async Task<bool> UpdateSessionPayloadAsync(
        string token, SessionPayload payload,
        CancellationToken cancellationToken = default)
    {
        Session? session = await Context.Sessions.FindAsync(new object[] { token }, cancellationToken: cancellationToken);
        if (session == null)
            return false;

        session.Payload = JsonSerializer.Serialize(payload);
        return true;
    }

    public async Task<bool> UpdateExpirationDateTimeAsync(string token, DateTimeOffset dateTime,
        CancellationToken cancellationToken = default)
    {
        Session? session = await Context.Sessions.FindAsync(new object[] { token }, cancellationToken: cancellationToken);
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
        Session? sessionEntity = await Context.Sessions.FindAsync(new object[] { token }, cancellationToken: cancellationToken);
        if (sessionEntity != null)
            Context.Remove(sessionEntity);
    }

    public async Task DeleteSessionAsync(IReadOnlySession session, CancellationToken cancellationToken = default)
    {
        Session? sessionEntity = await Context.Sessions.FindAsync(new object[] { session.Token }, cancellationToken: cancellationToken);
        if (sessionEntity != null)
            Context.Remove(session);
    }
}