
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

    public ITransaction? Current => _current;

    private EntityFrameworkTransaction? _current;
    
    public async Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_current != null)
            return _current.CreateSubordinateTransaction();

        _current = new EntityFrameworkTransaction(
            () => _current = null,
            await _context.Database.BeginTransactionAsync(cancellationToken));

        return _current;
    }
}