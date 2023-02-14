using Osekai.Octon.Models;
using Osekai.Octon.Persistence;
using Osekai.Octon.RichModels.Extensions;

namespace Osekai.Octon.RichModels;

public class App: ISavableEntity, IReadOnlyApp
{
    protected internal IUnitOfWork UnitOfWork { get; }

    protected internal App(
        int id, int order, string name, string simpleName, 
        bool visible, bool experimental, 
        IUnitOfWork unitOfWork)
    {
        Id = id;
        Order = order;
        Name = name;
        SimpleName = simpleName;
        Visible = visible;
        Experimental = experimental;
        UnitOfWork = unitOfWork;
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
            if (value.Length is < Specifications.AppSimpleNameMinLength or > Specifications.AppNameMaxLength)
                throw new ArgumentOutOfRangeException(nameof(value), value.Length, $"Invalid {nameof(Name)} length");

            _name = value;
        } 
    }

    
    public string SimpleName
    {
        get => _simpleName;

        set
        {
            if (value.Length is < Specifications.AppSimpleNameMinLength or > Specifications.AppNameMaxLength)
                throw new ArgumentOutOfRangeException(nameof(value), value.Length, $"Invalid {nameof(SimpleName)} length");
            
            _simpleName = value;
        }
    }

    public bool Visible { get; set; }
    public bool Experimental { get; set; }

    public Task PublishChangesAsync(CancellationToken cancellationToken = default)
        => UnitOfWork.AppRepository.SaveAppAsync(this, cancellationToken);

    public Task<AppTheme?> GetAppThemeAsync(CancellationToken cancellationToken = default) 
        => UnitOfWork.AppThemeRepository.GetAppThemeByAppIdAsync(Id).ContinueWith(t => t.Result?.ToRichModel());
}