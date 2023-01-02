namespace Osekai.Octon.Database;

public static class Specifications
{
    public const int AppNameMaxLength = 40;
    public const int AppNameMinLength = 1;

    public const int AppSimpleNameMaxLength = 20;
    public const int AppSimpleNameMinLength = 1;

    public const int AppColorMaxLength = 11;
    
    // 0,0,0 -> 5 characters
    public const int AppColorMinLength = 5;

    public const int HomeFaqTitleMaxLength = 255;
    public const int HomeFaqTitleMinLength = 1;
    public const int HomeFaqContentMaxLength = int.MaxValue;
    public const int HomeFaqContentMinLength = 1;
    public const int HomeFaqLocalizationPrefixMaxLength = 40;
    public const int HomeFaqLocalizationPrefixMinLength = 1;

    public const int SessionTokenLength = 32;
    public const int SessionTokenPayloadMaxLength = int.MaxValue;

    public const long SessionTokenMaxAgeInSeconds = 2592000;

    public const int CacheEntryNameMaxLength = 255;
    public const int CacheEntryNameMinLength = 1;
    public const int CacheEntryDataMaxLength = int.MaxValue;

    public const int MedalNameMaxLength = 50;
    public const int MedalNameMinLength = 1;

    public const int MedalLinkMaxLength = 255;
    public const int MedalLinkMinLength = 1;
    
    public const int MedalDescriptionMaxLength = 65535;
    public const int MedalDescriptionMinLength = 1;

    public const int MedalInstructionsMaxLength = 65535;
    public const int MedalInstructionsMinLength = 1;
    
    public const int MedalRestrictionMaxLength = 8;
    public const int MedalRestrictionMinLength = 1;

    public const int VideoMaxLength = 255;
    public const int VideoMinLength = 1;
    
    public const int GroupingMaxLength = 30;
    public const int GroupingMinLength = 1;
    
}