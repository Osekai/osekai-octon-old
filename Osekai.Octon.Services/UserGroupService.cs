using Osekai.Octon.Persistence;
using Osekai.Octon.Persistence.Dtos;
using Osekai.Octon.Services.Entities;
using Osekai.Octon.Services.Extensions;

namespace Osekai.Octon.Services;

public class UserGroupService
{
    protected IUnitOfWork UnitOfWork { get; }
    
    public UserGroupService(IUnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<UserGroup>> GetUserGroupsAsync(CancellationToken cancellationToken = default)
    {
        IEnumerable<UserGroupDto> userGroupDtos = await UnitOfWork.UserGroupRepository.GetUserGroups(cancellationToken);
        return userGroupDtos.Select(e => e.ToEntity(UnitOfWork));
    }
}