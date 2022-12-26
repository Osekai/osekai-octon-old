using Microsoft.Extensions.Options;
using Osekai.Octon.Options;

namespace Osekai.Octon.WebServer;

public class StaticUrlGenerator
{
    private OsuOAuthConfiguration _osuOAuthConfiguration;
    
    public StaticUrlGenerator(IOptions<OsuOAuthConfiguration> osuOAuthConfiguration)
    {
        _osuOAuthConfiguration = osuOAuthConfiguration.Value;
        _generatedString = new Dictionary<StaticUrlGeneratorString, string>();
        
        GenerateStrings();
    }

    public enum StaticUrlGeneratorString
    {
        OsuLoginString
    }

    private readonly Dictionary<StaticUrlGeneratorString, string> _generatedString;

    private string GenerateString(StaticUrlGeneratorString @string)
    {
        return @string switch
        {
            StaticUrlGeneratorString.OsuLoginString =>
                $"https://osu.ppy.sh/oauth/authorize?response_type=code&client_id={_osuOAuthConfiguration.ClientId}&scope=public&redirect_uri={_osuOAuthConfiguration.RedirectUri}",
            _ => throw new ArgumentOutOfRangeException(nameof(@string), @string, null)
        };
    }
    
    private void GenerateStrings()
    {
        StaticUrlGeneratorString[] strings = Enum.GetValues<StaticUrlGeneratorString>();
        
        foreach (var @string in strings)
            _generatedString.Add(@string, GenerateString(@string));
    }

    public string Get(StaticUrlGeneratorString @string) => _generatedString[@string];
}