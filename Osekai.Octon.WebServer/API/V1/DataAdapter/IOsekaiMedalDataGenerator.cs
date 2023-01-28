namespace Osekai.Octon.WebServer.API.V1.DataAdapter;

public interface IOsekaiMedalDataGenerator
{
    Task<IEnumerable<OsekaiMedalData>> GetOsekaiMedalDataAsync(CancellationToken cancellationToken);
}