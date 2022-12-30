using Microsoft.EntityFrameworkCore;
using Osekai.Octon.Database.Models;
using Osekai.Octon.Database.Repositories;
using Osekai.Octon.Database.Repositories.Query;

namespace Osekai.Octon.Database.EntityFramework.MySql.Repositories;

public class MySqlEntityFrameworkSessionRepository: ISessionRepository
{
    private readonly MySqlOsekaiDbContext _context;
    
    public MySqlEntityFrameworkSessionRepository(MySqlOsekaiDbContext context)
    {
        _context = context;
    }
    
    public Task<Session?> GetSessionFromTokenAsync(GetSessionByTokenQuery query, CancellationToken cancellationToken = default) =>
        _context.Sessions.AsNoTracking().Where(s => s.Token == query.Token).FirstOrDefaultAsync(cancellationToken);

    public async Task<Session> AddOrUpdateSessionAsync(AddOrUpdateSessionQuery query, CancellationToken cancellationToken = default)
    {
        Session? session = await _context.Sessions.Where(s => s.Token == query.Token).FirstOrDefaultAsync(cancellationToken);
        DateTime dateTime = DateTime.Now.AddSeconds(Specifications.SessionTokenMaxLifeInSeconds);
        
        if (session != null)
        {
            session.Payload = query.Payload;
            session.ExpiresAt = dateTime;
        }
        else
            _context.Add(
                new Session
                {
                    Token = query.Token, 
                    Payload = query.Payload,
                    ExpiresAt = dateTime
                });

        return new Session
        {
            Token = query.Token,
            Payload = query.Payload,
            ExpiresAt = dateTime
        };
    }

    public async Task<DateTimeOffset?> RefreshSessionAsync(RefreshSessionQuery query, CancellationToken cancellationToken = default)
    {
        Session? session = await _context.Sessions.Where(s => s.Token == query.Token).FirstOrDefaultAsync(cancellationToken);
        if (session == null)
            return null;

        DateTimeOffset expireAt = DateTime.Now.AddSeconds(Specifications.SessionTokenMaxLifeInSeconds);

        session.ExpiresAt = expireAt;
        return expireAt;
    }

    public Task<bool> SessionExists(SessionExistsQuery query, CancellationToken cancellationToken = default)
    {
        return _context.Sessions.AsNoTracking().AnyAsync(s => s.Token == query.Token, cancellationToken);
    }

    public async Task DeleteSessionAsync(DeleteSessionQuery query, CancellationToken cancellationToken = default)
    {
        Session? session = await _context.Sessions.Where(s => s.Token == query.Token).FirstOrDefaultAsync(cancellationToken);
        if (session != null)
            _context.Remove(session);
    }
}