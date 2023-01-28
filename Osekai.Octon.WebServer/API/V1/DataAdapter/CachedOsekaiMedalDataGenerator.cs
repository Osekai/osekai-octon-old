using Osekai.Octon.Services.Entities;

namespace Osekai.Octon.WebServer.API.V1.DataAdapter;

public class CachedOsekaiMedalDataGenerator: IOsekaiMedalDataGenerator
{
    private readonly IOsekaiMedalDataGenerator _osekaiMedalDataGenerator;
    private readonly ICache _cache;

    public CachedOsekaiMedalDataGenerator(ICache cache,
        IOsekaiMedalDataGenerator osekaiMedalDataGenerator)
    {
        _osekaiMedalDataGenerator = osekaiMedalDataGenerator;
        _cache = cache;
    } 
    
    public async Task<IEnumerable<OsekaiMedalData>> GetOsekaiMedalDataAsync(CancellationToken cancellationToken)
    {
        OsekaiMedalData[]? medalData = await _cache.GetAsync<OsekaiMedalData[]>("data_adapter_medals");

        if (medalData == null)
        {
            medalData = (await _osekaiMedalDataGenerator.GetOsekaiMedalDataAsync(cancellationToken)).ToArray();
            await _cache.SetAsync("data_adapter_medals", medalData, 86400, cancellationToken);
        }
        
        return medalData;
    }
}