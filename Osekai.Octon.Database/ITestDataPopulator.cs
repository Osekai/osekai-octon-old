namespace Osekai.Octon.Database;

public interface ITestDataPopulator
{
    Task PopulateDatabaseAsync(CancellationToken cancellationToken = default);
}