namespace Osekai.Octon.Persistence;

public interface ITestDataPopulator
{
    Task PopulateDatabaseAsync(CancellationToken cancellationToken = default);
}