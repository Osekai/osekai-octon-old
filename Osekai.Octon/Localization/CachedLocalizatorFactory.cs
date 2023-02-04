namespace Osekai.Octon.Localization;

public class CachedLocalizatorFactory: ILocalizatorFactory
{
    protected ILocalizatorFactory LocalizatorFactory { get; }
    protected Dictionary<string, ILocalizator> CachedLocalizators { get; }

    public CachedLocalizatorFactory(ILocalizatorFactory localizatorFactory)
    {
        LocalizatorFactory = localizatorFactory;
        CachedLocalizators = new Dictionary<string, ILocalizator>();
    }
    
    public ILocalizator CreateLocalizatorFromLanguageCode(string code)
    {
        if (!CachedLocalizators.TryGetValue(code, out ILocalizator? localizator))
        {
            localizator = LocalizatorFactory.CreateLocalizatorFromLanguageCode(code);
            CachedLocalizators.Add(code, localizator);
        }

        return localizator;
    }
}