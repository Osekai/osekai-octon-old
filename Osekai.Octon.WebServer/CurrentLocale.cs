using Osekai.Octon.Localization;

namespace Osekai.Octon.WebServer;

public class CurrentLocale
{
    public string LanguageCode { get; private set; } = null!;

    protected ILocalizatorFactory LocalizatorFactory { get; }
    public ILocalizator Localizator { get; private set; } = null!;

    public CurrentLocale(ILocalizatorFactory localizatorFactory)
    {
        LocalizatorFactory = localizatorFactory;
        Set("en_GB");
    }

    public void Set(string languageCode)
    {
        LanguageCode = languageCode;
        Localizator = LocalizatorFactory.CreateLocalizatorFromLanguageCode(languageCode);
    }
    
}