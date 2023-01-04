namespace Osekai.Octon.DataAdapter;

public class CachedOsekaiDataAdapter: IOsekaiDataAdapter
{
    private readonly IOsekaiDataAdapter _osekaiDataAdapter;
    private readonly ICache _cache;

    public CachedOsekaiDataAdapter(ICache cache,
        IOsekaiDataAdapter osekaiDataAdapter)
    {
        _osekaiDataAdapter = osekaiDataAdapter;
        _cache = cache;
    } 
    
    public async Task<IEnumerable<OsekaiMedalData>> GetMedalDataAsync(CancellationToken cancellationToken)
    {
        IReadOnlyList<OsekaiMedalData>? medalData = await _cache.GetAsync<OsekaiMedalData[]>("data_adapter_medals");

        if (medalData == null)
        {
            medalData = await _osekaiDataAdapter.GetMedalDataAsync(cancellationToken).ContinueWith(r => r.Result.ToArray());
            await _cache.SetAsync("data_adapter_medals", medalData, 600, cancellationToken);
        }
        
        return medalData;
    }
}