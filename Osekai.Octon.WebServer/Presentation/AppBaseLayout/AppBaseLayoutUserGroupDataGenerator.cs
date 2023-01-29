using System.Drawing;
using System.Text;
using Microsoft.Extensions.ObjectPool;
using Osekai.Octon.Persistence;
using Osekai.Octon.Services;
using Osekai.Octon.Services.Entities;

namespace Osekai.Octon.WebServer.Presentation.AppBaseLayout;

public class AppBaseLayoutUserGroupDataGenerator: IAppBaseLayoutUserGroupDataGenerator
{
    protected UserGroupService UserGroupService { get; }
    protected ObjectPool<StringBuilder> StringBuilderObjectPool { get; }
    
    public AppBaseLayoutUserGroupDataGenerator(ObjectPool<StringBuilder> stringBuilderObjectPool, UserGroupService userGroupService)
    {
        UserGroupService = userGroupService;
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
    
    public async Task<IEnumerable<AppBaseLayoutUserGroupData>> GenerateAsync(CancellationToken cancellationToken = default)
    {
        IEnumerable<UserGroup> userGroups = await UserGroupService.GetUserGroupsAsync(cancellationToken);
        return userGroups.Select(e => new AppBaseLayoutUserGroupData
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