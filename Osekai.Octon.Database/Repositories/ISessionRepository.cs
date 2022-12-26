﻿using Osekai.Octon.Database.Models;
using Osekai.Octon.Database.Repositories.Query;

namespace Osekai.Octon.Database.Repositories;

public interface ISessionRepository
{
    Task<Session?> GetSessionFromTokenAsync(GetSessionByTokenQuery query, CancellationToken cancellationToken = default);
    Task<Session> AddOrUpdateSessionAsync(AddOrUpdateSessionQuery query, CancellationToken cancellationToken = default);
    Task<bool> SessionExists(SessionExistsQuery query, CancellationToken cancellationToken = default);
    Task DeleteSessionAsync(DeleteSessionQuery query, CancellationToken cancellationToken = default);
}