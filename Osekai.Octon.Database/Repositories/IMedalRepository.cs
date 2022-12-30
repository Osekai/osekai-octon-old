using System.Linq.Expressions;
using Osekai.Octon.Database.Models;

namespace Osekai.Octon.Database.Repositories;

public interface IMedalRepository
{
    Task<IReadOnlyCollection<Medal>> GetMedalsAsync(Expression<Func<Medal, bool>>? filter = null, long offset  = 0, long limit = long.MaxValue, CancellationToken cancellationToken = default);
}   