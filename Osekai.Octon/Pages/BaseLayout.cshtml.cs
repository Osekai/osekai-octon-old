using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Osekai.Octon.Pages;

public abstract class BaseLayout : PageModel
{
    public abstract string MetadataDescription { get; }
    public abstract string MetadataTitle { get; }
    public abstract string MetadataThemeColor { get; }
    public abstract IReadOnlyCollection<string> MetadataKeywords { get; }
    public abstract string MetadataUrl { get; }
}