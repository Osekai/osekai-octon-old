using Osekai.Octon.Persistence.EntityFramework.MySql.Dtos;

namespace Osekai.Octon.Persistence.EntityFramework.MySql.Entities;

public class Locale
{
    public string Name { get; set; }= null!;
    public string Code { get; set; }= null!;
    public string Short { get; set; } = null!;
    public string Flag { get; set; } = null!;
    public bool Experimental { get; set; } 
    public bool Wip { get; set; }
    public bool Rtl { get; set; }
    public string? ExtraHtml { get; set; } = null!;
    public string? ExtraCss { get; set; } = null!;

    public LocaleDto ToDto()
    {
        return new LocaleDto(Name, Code, Short, new Uri(Flag), Experimental, Wip, Rtl, ExtraHtml, ExtraCss);
    }
}