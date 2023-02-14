using Osekai.Octon.Models;

namespace Osekai.Octon.Persistence.EntityFramework.MySql.Dtos;

public class LocaleDto: IReadOnlyLocale
{
    public LocaleDto(string name, string code, string @short, Uri flag, bool experimental, bool wip, bool rtl, string? extraHtml, string? extraCss)
    {
        Name = name;
        Code = code;
        Short = @short;
        Flag = flag;
        Experimental = experimental;
        Wip = wip;
        Rtl = rtl;
        ExtraHtml = extraHtml;
        ExtraCss = extraCss;
    }

    public string Name { get; }
    public string Code { get; }
    public string Short { get; }
    public Uri Flag { get; }
    public bool Experimental { get; }
    public bool Wip { get; }
    public bool Rtl { get; }
    public string? ExtraHtml { get; }
    public string? ExtraCss { get; }
}