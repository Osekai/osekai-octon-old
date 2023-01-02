using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Osekai.Octon.Database.Dtos;
using Osekai.Octon.Database.EntityFramework.MySql.Models;
using Osekai.Octon.Database.HelperTypes;
using Osekai.Octon.Database.Repositories;

namespace Osekai.Octon.Database.EntityFramework.MySql.Repositories;

public class MySqlEntityFrameworkSessionRepository: ISessionRepository
{
    private readonly MySqlOsekaiDbContext _context;
    
    public MySqlEntityFrameworkSessionRepository(MySqlOsekaiDbContext context)
    {
        _context = context;
    }

    public async Task<SessionDto?> GetSessionFromTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        Session? session = await _context.Sessions.AsNoTracking().Where(s => s.Token == token && DateTimeOffset.Now < s.ExpiresAt).FirstOrDefaultAsync(cancellationToken);
        return session?.ToDto();
    }

    public Task<SessionDto> AddSessionAsync(string token, SessionDtoPayload payload, DateTimeOffset expiresAt, CancellationToken cancellationToken = default)
    {
        _context.Sessions.Add(new Session
        {
            Token = token, 
            ExpiresAt = expiresAt, 
            Payload = JsonSerializer.Serialize(payload)
        });
        
        return Task.FromResult(new SessionDto(token, payload, expiresAt));
    }

    public async Task<bool> UpdateSessionPayloadAsync(
        string token, SessionDtoPayload payload,
        CancellationToken cancellationToken = default)
    {
        Session? session = await _context.Sessions.Where(s => s.Token == token).FirstOrDefaultAsync(cancellationToken);
        if (session == null)
            return false;

        session.Payload = JsonSerializer.Serialize(payload);
        return true;
    }

    public async Task<bool> UpdateExpirationDateTimeAsync(string token, DateTimeOffset dateTime,
        CancellationToken cancellationToken = default)
    {
        Session? session = await _context.Sessions.Where(s => s.Token == token).FirstOrDefaultAsync(cancellationToken);
        if (session == null)
            return false;

        session.ExpiresAt = dateTime;
        return true;
    }

    public Task<bool> SessionExists(string token, CancellationToken cancellationToken = default)
    {
        return _context.Sessions.AsNoTracking().AnyAsync(s => s.Token == token, cancellationToken);
    }

    public async Task DeleteSessionAsync(string token, CancellationToken cancellationToken = default)
    {
        Session? session = await _context.Sessions.Where(s => s.Token == token).FirstOrDefaultAsync(cancellationToken);
        if (session != null)
            _context.Remove(session);
    }
}