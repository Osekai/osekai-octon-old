using Microsoft.EntityFrameworkCore;
using Osekai.Octon.Domain.AggregateRoots;
using Osekai.Octon.Domain.Repositories;
using Osekai.Octon.Domain.ValueObjects;
using UserGroup = Osekai.Octon.Persistence.EntityFramework.MySql.Entities.UserGroup;

namespace Osekai.Octon.Persistence.EntityFramework.MySql.Repositories;

public class MySqlEntityFrameworkTeamMemberRepository: ITeamMemberRepository
{
    protected MySqlOsekaiDbContext Context { get; }
    
    public MySqlEntityFrameworkTeamMemberRepository(MySqlOsekaiDbContext context)
    {
        Context = context;
    }
    
    public async Task<IEnumerable<TeamMember>> GetTeamMembersAsync(CancellationToken cancellationToken = default)
    {
        Entities.TeamMember[] teamMembers = await Context.TeamMembers.ToArrayAsync(cancellationToken);
        int[] teamMemberIds = teamMembers.Select(x => x.UserId).ToArray();

        Dictionary<int, UserGroup[]> userGroupsPerTeamMember = await Context.UserGroupsForUsers
            .Include(g => g.UserGroup)
            .Where(g => teamMemberIds.Contains(g.UserId))
            .Select(g => new { g.UserGroup, g.UserId })
            .GroupBy(g => g.UserId)
            .ToDictionaryAsync(t => t.Key, t => t.Select(u => u.UserGroup).ToArray(), cancellationToken);

        return teamMembers.Select(t =>
        {
            TeamMember tNew = t.ToAggregateRoot();

            if (userGroupsPerTeamMember.TryGetValue(t.UserId, out UserGroup[]? userGroups))
                tNew.UserGroups = new Ref<IReadOnlyList<Domain.AggregateRoots.UserGroup>>(userGroups.Select(u => u.ToAggregateRoot()).ToArray());
            else
                tNew.UserGroups = new Ref<IReadOnlyList<Domain.AggregateRoots.UserGroup>>(Array.Empty<Domain.AggregateRoots.UserGroup>());

            tNew.Socials = new Ref<IReadOnlyList<Social>>(t.Socials.ToArray());
            
            return tNew;
        });
    }
}