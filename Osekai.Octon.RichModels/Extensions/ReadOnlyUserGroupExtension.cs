using Osekai.Octon.Models;
using Osekai.Octon.Persistence;

namespace Osekai.Octon.RichModels.Extensions;

public static class ReadOnlyUserGroupExtension
{
    public static UserGroup ToRichModel(this IReadOnlyUserGroup userGroup, IUnitOfWork unitOfWork)
    {
        return new UserGroup(userGroup.Id,
            userGroup.Name,
            userGroup.ShortName,
            userGroup.Permissions,
            userGroup.Description,
            userGroup.Colour,
            userGroup.Order,
            userGroup.Hidden,
            userGroup.ForceVisibleInComments,
            unitOfWork);
    }
}