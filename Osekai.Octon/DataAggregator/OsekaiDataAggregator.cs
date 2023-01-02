using Osekai.Octon.OsuApi;

namespace Osekai.Octon.DataAggregator;

public class OsekaiDataAggregator
{
    private IAuthenticatedOsuApiV2Interface _osuApiV2Interface;
    
    public OsekaiDataAggregator(IAuthenticatedOsuApiV2Interface osuApiV2Interface)
    {
        _osuApiV2Interface = osuApiV2Interface;
    }

    public async Task GetAggregatedMedalData()
    {
        
    }
}