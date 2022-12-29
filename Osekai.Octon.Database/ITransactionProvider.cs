using System.Data;

namespace Osekai.Octon.Database;

public interface ITransactionProvider
{
    ITransaction? Current { get; }
    
    Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
}