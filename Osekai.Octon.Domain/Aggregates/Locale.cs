using System.Text.RegularExpressions;

namespace Osekai.Octon.Domain.Aggregates;

public class Locale
{
    private string _name = null!;
    private string _code = null!;
    private string _shortName = null!;
    private Uri _flag = null!;
    private string? _extraHtml;
    private string? _extraCss;

    public Locale(string name, string code, string shortName, Uri flag, string? extraHtml, string? extraCss, bool experimental, bool wip, bool rtl)
    {
        Name = name;
        Code = code;
        ShortName = shortName;
        Flag = flag;
        ExtraHtml = extraHtml;
        ExtraCss = extraCss;
        Experimental = experimental;
        Wip = wip;
        Rtl = rtl;
    }

    public string Name
    {
        get => _name;
        set
        {
            if (value.Length is < Specifications.LocaleNameMinLength or > Specifications.LocaleNameMaxLength)
                throw new ArgumentOutOfRangeException(nameof(value), value.Length, $"Invalid {nameof(Name)} length");

            _name = value;
        }
    }

    public string Code
    {
        get => _code;
        set
        {
            if (value.Length is < Specifications.LocaleNameMinLength or > Specifications.LocaleNameMaxLength)
                throw new ArgumentOutOfRangeException(nameof(value), value.Length, $"Invalid {nameof(Code)} length");
            
            if (!Regex.IsMatch(value, @"[a-z]{2}_[A-Z]{2}"))
                throw new FormatException($"Invalid format for {nameof(Code)}");

            _code = value;
        }
    }

    public string ShortName
    {
        get => _shortName;
        set
        {
            if (value.Length != Specifications.LocaleShortLength)
                throw new ArgumentOutOfRangeException(nameof(value), value.Length, $"Invalid {nameof(ShortName)} length");

            _shortName = value;
        }

    }

    public Uri Flag
    {
        get => _flag;
        set
        {
            //if (value.ToString().Length is < Specifications.LocaleFlagMinLength or > Specifications.LocaleFlagMaxLength)
                //throw new ArgumentOutOfRangeException(nameof(value), $"Invalid {nameof(Flag)} length");

            _flag = value;
        }
    }

    public bool Experimental { get; set; }
    public bool Wip { get; set; }
    public bool Rtl { get; set; }

    public string? ExtraHtml
    {
        get => _extraHtml;
        set
        {
            if (value?.Length is < Specifications.LocaleExtraHtmlMinLength or > Specifications.LocaleExtraHtmlMaxLength)
                throw new ArgumentOutOfRangeException(nameof(value), $"Invalid {nameof(ExtraHtml)} length");

            _extraHtml = value;
        }
    }

    public string? ExtraCss
    {
        get => _extraCss;
        set
        {
            if (value?.Length is < Specifications.LocaleExtraCssMinLength or > Specifications.LocaleExtraCssMaxLength)
                throw new ArgumentOutOfRangeException(nameof(value), $"Invalid {nameof(ExtraCss)} length");

            _extraCss = value;
        }
    }
}