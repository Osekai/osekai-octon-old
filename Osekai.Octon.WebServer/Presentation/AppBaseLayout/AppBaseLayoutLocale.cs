using System.Text.Json.Serialization;

namespace Osekai.Octon.WebServer.Presentation.AppBaseLayout;

public class AppBaseLayoutLocale
{
    public AppBaseLayoutLocale() {}
    
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

    public string Name { get; set; } = null!;
    public string Code { get; set; } = null!; 
    public string Short { get; set; }= null!;
    public string Flag { get; set; }= null!;
    public bool Experimental { get; set; }
    public bool Wip { get; set; }
    public bool Rtl { get; set; }
    public string? ExtraHtml { get; set; }
    public string? ExtraCss { get; set; }
}