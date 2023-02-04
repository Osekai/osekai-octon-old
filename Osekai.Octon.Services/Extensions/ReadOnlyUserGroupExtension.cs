using Osekai.Octon.Objects;
using Osekai.Octon.Persistence;
using Osekai.Octon.Services.Entities;

namespace Osekai.Octon.Services.Extensions;

public static class ReadOnlyUserGroupExtension
{
    public static UserGroup ToEntity(this IReadOnlyUserGroup userGroup, IUnitOfWork unitOfWork)
    {
        return new UserGroup(userGroup.Id,
            userGroup.Name,
            userGroup.ShortName,
            unitOfWork,
            userGroup.Permissions,
            userGroup.Description,
            userGroup.Colour,
            userGroup.Order,
            userGroup.Hidden,
            userGroup.ForceVisibleInComments);
    }
}