using Osekai.Octon.Domain.Entities;

namespace Osekai.Octon.Domain.Aggregates;

public class App
{
    public App(string name, string simpleName, int id, int order, bool visible, bool experimental)
    {
        Name = name;
        SimpleName = simpleName;
        Id = id;
        Order = order;
        Visible = visible;
        Experimental = experimental;
        AppTheme = new Ref<AppTheme?>();
        Faqs = new Ref<IReadOnlyList<HomeFaq>>();
    }
    
    public int Id { get; }
    public int Order { get; set; }
    
    /* _name and _simpleName are set to null! because they are initially set by Name and SimpleName.
    There's no need to keep the warning. */
    
    private string _name = null!;
    private string _simpleName = null!;

    public string Name
    {
        get => _name;
        
        set
        {
            if (value.Length is < Specifications.AppNameMinLength or > Specifications.AppNameMaxLength)
                throw new ArgumentOutOfRangeException(nameof(value), value.Length, $"Invalid {nameof(Name)} length");

            _name = value;
        } 
    }

    
    public string SimpleName
    {
        get => _simpleName;

        set
        {
            if (value.Length is < Specifications.AppSimpleNameMinLength or > Specifications.AppSimpleNameMaxLength)
                throw new ArgumentOutOfRangeException(nameof(value), value.Length, $"Invalid {nameof(SimpleName)} length");
            
            _simpleName = value;
        }
    }

    public bool Visible { get; set; }
    public bool Experimental { get; set; }
    public Ref<AppTheme?> AppTheme { get; set; }
    
    public Ref<IReadOnlyList<HomeFaq>> Faqs { get; set; }
}