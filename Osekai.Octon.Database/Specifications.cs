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
}