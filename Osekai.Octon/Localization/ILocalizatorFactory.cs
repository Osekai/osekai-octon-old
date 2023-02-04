namespace Osekai.Octon.Localization;

public interface ILocalizatorFactory
{
    ILocalizator CreateLocalizatorFromLanguageCode(string code);

    public ILocalizator CreateLocalizator() => CreateLocalizatorFromLanguageCode("en_GB");
}