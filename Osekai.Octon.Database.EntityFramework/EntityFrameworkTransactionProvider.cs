
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace Osekai.Octon.Database.EntityFramework;

public class EntityFrameworkTransactionProvider : ITransactionProvider
{
    private readonly DbContext _context;
    
    public EntityFrameworkTransactionProvider(DbContext context)
    {
        _context = context;
    }

    public async Task<ITransaction> BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.Serializable,
        CancellationToken cancellationToken = default)
    {
        return new EntityFrameworkTransaction(
            await _context.Database.BeginTransactionAsync(isolationLevel, cancellationToken));
    }
}