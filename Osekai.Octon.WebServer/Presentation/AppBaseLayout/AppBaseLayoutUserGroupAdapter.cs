using System.Drawing;
using System.Text;
using Microsoft.Extensions.ObjectPool;
using Osekai.Octon.Models;

namespace Osekai.Octon.WebServer.Presentation.AppBaseLayout;

public class AppBaseLayoutUserGroupAdapter: IAdapter<IReadOnlyUserGroup, AppBaseLayoutUserGroup>
{
    protected ObjectPool<StringBuilder> StringBuilderObjectPool { get; }
    
    public AppBaseLayoutUserGroupAdapter(ObjectPool<StringBuilder> stringBuilderObjectPool)
    {
        StringBuilderObjectPool = stringBuilderObjectPool;
    }

    private string GetColourString(Color color)
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
    
    public Task<AppBaseLayoutUserGroup> AdaptAsync(IReadOnlyUserGroup e, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new AppBaseLayoutUserGroup
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