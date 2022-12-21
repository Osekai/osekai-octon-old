using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Osekai.Octon.Pages;

public abstract class BaseLayout : PageModel
{
    public abstract string MetadataDescription { get; }
    public abstract string MetadataTitle { get; }
    public abstract string MetadataThemeColor { get; }
    
    private static readonly IReadOnlyCollection<string> DefaultMetadataKeywords = 
        new string[] { "osekai", "medals","osu","achievements","rankings","alternative","medal rankings","osekai","the","home","of","more" };

    public virtual IReadOnlyCollection<string> MetadataKeywords => DefaultMetadataKeywords;
    public abstract string MetadataUrl { get; }
        
    public virtual IReadOnlyList<string> CustomStylesheets { get; } = Array.Empty<string>();
}