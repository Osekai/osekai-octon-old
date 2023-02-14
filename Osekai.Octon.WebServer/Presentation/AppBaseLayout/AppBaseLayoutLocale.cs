namespace Osekai.Octon.WebServer.Presentation.AppBaseLayout;

public class AppBaseLayoutLocale
{
    public AppBaseLayoutLocale(string name, string code, string s, string flag, bool experimental, bool wip, bool rtl, string? extraHtml, string? extraCss)
    {
        Name = name;
        Code = code;
        Short = s;
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
    public string Flag { get; }
    public bool Experimental { get; }
    public bool Wip { get; }
    public bool Rtl { get; }
    public string? ExtraHtml { get; }
    public string? ExtraCss { get; }
}