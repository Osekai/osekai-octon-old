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
        if (await SessionExists(new SessionExistsQuery(query.Token), cancellationToken))
            _context.Entry(
                new Session
                {
                    Token = query.Token,
                    Payload = query.Payload,
                    ExpiresAt = DateTime.Now.AddSeconds(Specifications.SessionTokenMaxLifeInSeconds)
                }).State = EntityState.Modified;
        else
            _context.Add(
                new Session
                {
                    Token = query.Token, 
                    Payload = query.Payload,
                    ExpiresAt = DateTime.Now.AddSeconds(Specifications.SessionTokenMaxLifeInSeconds)
                });

        return new Session
        {
            Token = query.Token,
            Payload = query.Payload,
            ExpiresAt = DateTime.Now.AddSeconds(Specifications.SessionTokenMaxLifeInSeconds)
        };
    }

    public Task<bool> SessionExists(SessionExistsQuery query, CancellationToken cancellationToken = default)
    {
        return _context.Sessions.AsNoTracking().AnyAsync(s => s.Token == query.Token, cancellationToken);
    }

    public Task DeleteSessionAsync(DeleteSessionQuery query, CancellationToken cancellationToken = default)
    {
        _context.Entry(new Session { Token = query.Token }).State = EntityState.Deleted;
        return Task.CompletedTask;
    }
}