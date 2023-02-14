using Osekai.Octon.Models;
using Osekai.Octon.Persistence;
using Osekai.Octon.RichModels;
using Osekai.Octon.RichModels.Extensions;

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
        IEnumerable<IReadOnlyUserGroup> userGroupDtos = await UnitOfWork.UserGroupRepository.GetUserGroups(cancellationToken);
        return userGroupDtos.Select(e => e.ToRichModel(UnitOfWork));
    }
}