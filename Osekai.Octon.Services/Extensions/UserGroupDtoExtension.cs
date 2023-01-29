using Osekai.Octon.Persistence;
using Osekai.Octon.Persistence.Dtos;
using Osekai.Octon.Services.Entities;

namespace Osekai.Octon.Services.Extensions;

public static class UserGroupDtoExtension
{
    public static UserGroup ToEntity(this UserGroupDto userGroupDto, IUnitOfWork unitOfWork)
    {
        return new UserGroup(userGroupDto.Id,
            userGroupDto.Name,
            userGroupDto.ShortName,
            unitOfWork,
            userGroupDto.Permissions,
            userGroupDto.Description,
            userGroupDto.Colour,
            userGroupDto.Order,
            userGroupDto.Hidden,
            userGroupDto.ForceVisibleInComments);
    }
}