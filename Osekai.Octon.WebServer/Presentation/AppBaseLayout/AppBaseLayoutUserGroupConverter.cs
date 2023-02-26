using System.Drawing;
using System.Text;
using Microsoft.Extensions.ObjectPool;
using Osekai.Octon.Domain.AggregateRoots;
using Osekai.Octon.Drawing;

namespace Osekai.Octon.WebServer.Presentation.AppBaseLayout;

public class AppBaseLayoutUserGroupConverter: IConverter<UserGroup, AppBaseLayoutUserGroup>
{
    protected ObjectPool<StringBuilder> StringBuilderObjectPool { get; }
    
    public AppBaseLayoutUserGroupConverter(ObjectPool<StringBuilder> stringBuilderObjectPool)
    {
        StringBuilderObjectPool = stringBuilderObjectPool;
    }

    private string GetColourString(RgbColour color)
    {
        StringBuilder stringBuilder = StringBuilderObjectPool.Get();
        try
        {
            stringBuilder.AppendFormat("{0},{1},{2}", color.R, color.B, color.G);
            return stringBuilder.ToString();
        }
        finally
        {
            StringBuilderObjectPool.Return(stringBuilder);
        }
    }
    
    public ValueTask<AppBaseLayoutUserGroup> AdaptAsync(UserGroup e, CancellationToken cancellationToken = default)
    {
        return ValueTask.FromResult(new AppBaseLayoutUserGroup
        {
            Colour = GetColourString(e.Colour),
            Description = e.Description,
            Hidden = e.Hidden,
            Id = e.Id,
            Name = e.Name,
            Order = e.Order,
            ForceVisible = e.ForceVisibleInComments,
            ShortName = e.ShortName
        });
    }
}