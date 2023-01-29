namespace Osekai.Octon.WebServer.Presentation.AppBaseLayout;

public class CachedAppBaseLayoutMedalDataGenerator: IAppBaseLayoutMedalDataGenerator
{
    private readonly IAppBaseLayoutMedalDataGenerator _appBaseLayoutMedalDataGenerator;
    private readonly ICache _cache;

    public CachedAppBaseLayoutMedalDataGenerator(ICache cache,
        IAppBaseLayoutMedalDataGenerator appBaseLayoutMedalDataGenerator)
    {
        _appBaseLayoutMedalDataGenerator = appBaseLayoutMedalDataGenerator;
        _cache = cache;
    } 
    
    public async Task<IEnumerable<AppBaseLayoutMedalData>> GenerateAsync(CancellationToken cancellationToken)
    {
        AppBaseLayoutMedalData[]? medalData = await _cache.GetAsync<AppBaseLayoutMedalData[]>("data_adapter_medals");

        if (medalData == null)
        {
            medalData = (await _appBaseLayoutMedalDataGenerator.GenerateAsync(cancellationToken)).ToArray();
            await _cache.SetAsync("data_adapter_medals", medalData, 86400, cancellationToken);
        }
        
        return medalData;
    }
}