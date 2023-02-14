namespace Osekai.Octon.Models;

public interface IReadOnlyLocale
{
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