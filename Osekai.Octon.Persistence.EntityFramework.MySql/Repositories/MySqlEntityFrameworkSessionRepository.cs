using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Osekai.Octon.Objects;
using Osekai.Octon.Persistence.EntityFramework.MySql.Dtos;
using Osekai.Octon.Persistence.EntityFramework.MySql.Models;
using Osekai.Octon.Persistence.HelperTypes;
using Osekai.Octon.Persistence.Repositories;

namespace Osekai.Octon.Persistence.EntityFramework.MySql.Repositories;

public class MySqlEntityFrameworkSessionRepository: ISessionRepository
{
    private MySqlOsekaiDbContext Context { get; }
    
    public MySqlEntityFrameworkSessionRepository(MySqlOsekaiDbContext context)
    {
        Context = context;
    }

    public async Task<IReadOnlySession?> GetSessionFromTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        Session? session = await Context.Sessions.AsNoTracking().Where(s => s.Token == token && DateTimeOffset.Now < s.ExpiresAt).FirstOrDefaultAsync(cancellationToken);
        return session?.ToDto();
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

    public Task<bool> SessionExists(string token, CancellationToken cancellationToken = default)
    {
        return Context.Sessions.AsNoTracking().AnyAsync(s => s.Token == token, cancellationToken);
    }

    public async Task DeleteSessionAsync(string token, CancellationToken cancellationToken = default)
    {
        Session? session = await Context.Sessions.FindAsync(new object[] { token }, cancellationToken: cancellationToken);
        if (session != null)
            Context.Remove(session);
    }
}