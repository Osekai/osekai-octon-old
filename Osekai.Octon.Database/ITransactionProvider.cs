namespace Osekai.Octon.Database;

public interface ITransactionProvider
{
    Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken);
}