﻿namespace Osekai.Octon.Domain;

public static class Specifications
{
    public const int AppNameMaxLength = 40;
    public const int AppNameMinLength = 1;

    public const int AppSimpleNameMaxLength = 20;
    public const int AppSimpleNameMinLength = 1;

    public const int HomeFaqTitleMaxLength = 255;
    public const int HomeFaqTitleMinLength = 1;
    public const int HomeFaqContentMaxLength = int.MaxValue;
    public const int HomeFaqContentMinLength = 1;
    public const int HomeFaqLocalizationPrefixMaxLength = 40;
    public const int HomeFaqLocalizationPrefixMinLength = 1;

    public const int SessionTokenLength = 32;

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

    public const int MedalVideoMaxLength = 255;
    public const int MedalVideoMinLength = 1;
    
    public const int MedalGroupingMaxLength = 30;
    public const int MedalGroupingMinLength = 1;

    public const int FirstAchievedByMaxLength = 255;
    public const int FirstAchievedByMinLength = 1;

    public const int MedalSolutionSubmittedByMaxLength = 50;
    public const int MedalSolutionSubmittedByMinLength = 1;
    
    public const int MedalSolutionTextMaxLength = 65535;
    public const int MedalSolutionTextMinLength = 0;
    
    public const int GroupNameMaxLength = 100;
    public const int GroupNameMinLength = 1;

    public const int GroupShortNameMaxLength = 100;
    public const int GroupShortNameMinLength = 1;

    public const int GroupDescriptionMaxLength = ushort.MaxValue - 1;
    public const int GroupDescriptionMinLength = 1;
    
    public const int LocaleNameMaxLength = 100;
    public const int LocaleNameMinLength = 0;
    
    public const int LocaleCodeLength = 5;
    public const int LocaleShortLength = 2;
    
    public const int LocaleFlagMaxLength = 255;
    public const int LocaleFlagMinLength = 1;
    
    public const int LocaleExtraHtmlMaxLength = int.MaxValue;
    public const int LocaleExtraHtmlMinLength = 1;
    
    public const int LocaleExtraCssMaxLength = int.MaxValue;
    public const int LocaleExtraCssMinLength = 1;

    public const int SocialNameMaxLength = 50;
    public const int SocialNameMinLength = 1;

    public const int SocialLinkMaxLength = 255;
    public const int SocialLinkMinLength = 1;
    
    public const int SocialIconMaxLength = 50;
    public const int SocialIconMinLength = 1;

    public const int TeamMemberRoleMaxLength = 100;
    public const int TeamMemberRoleMinLength = 1;

    public const int TeamMemberNameMaxLength = 100;
    public const int TeamMemberNameMinLength = 1;
    
    public const int TeamMemberNameAltMaxLength = 100;
    public const int TeamMemberNameAltMinLength = 1;
}