namespace Osekai.Octon.DataAdapter;

public interface IOsekaiDataAdapter
{
    Task<IEnumerable<OsekaiMedalData>> GetMedalDataAsync(CancellationToken cancellationToken);
}