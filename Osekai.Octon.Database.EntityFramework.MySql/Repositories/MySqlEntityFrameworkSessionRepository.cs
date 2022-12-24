using System.Security.Principal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Osekai.Octon.Database.Models;
using Osekai.Octon.Database.Repositories;
using Osekai.Octon.Database.Repositories.Query;

namespace Osekai.Octon.Database.EntityFramework.Repositories;

public class MySqlEntityFrameworkSessionRepository: ISessionRepository
{
    private readonly MySqlOsekaiDbContext _context;
    
    public MySqlEntityFrameworkSessionRepository(MySqlOsekaiDbContext context)
    {
        _context = context;
    }
    
    public Task<Session?> GetSessionFromTokenAsync(GetSessionByTokenQuery query, CancellationToken cancellationToken = default) =>
        _context.Sessions.AsNoTracking().Where(s => s.Token == query.Token).FirstOrDefaultAsync(cancellationToken);

    public Task<Session> AddOrReplaceSessionAsync(AddOrReplaceSessionQuery query, CancellationToken cancellationToken = default)
    {
        return _context.Database.SqlQueryRaw<Session>(
            "REPLACE INTO Sessions (Token, Payload) VALUES ({0}, {1}); SELECT * FROM Sessions WHERE Token = {1}", query.Token, query.Payload)
            .FirstAsync(cancellationToken);
    }

    public Task<bool> SessionExists(SessionExistsQuery query, CancellationToken cancellationToken = default) =>
        _context.Sessions.AsNoTracking().AnyAsync(s => s.Token == query.Token, cancellationToken);
}