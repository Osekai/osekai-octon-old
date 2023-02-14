using Microsoft.EntityFrameworkCore;
using Osekai.Octon.Models;
using Osekai.Octon.Persistence.EntityFramework.MySql.Dtos;
using Osekai.Octon.Persistence.EntityFramework.MySql.Entities;
using Osekai.Octon.Persistence.Repositories;

namespace Osekai.Octon.Persistence.EntityFramework.MySql.Repositories;

public class MySqlEntityFrameworkMedalSolutionRepository: IMedalSolutionRepository
{
    private MySqlOsekaiDbContext Context { get; }
    
    public MySqlEntityFrameworkMedalSolutionRepository(MySqlOsekaiDbContext context)
    {
        Context = context;
    }
    
    public async Task<IReadOnlyMedalSolution?> GetMedalSolutionByMedalIdAsync(int medalId, CancellationToken cancellationToken = default)
    {
        MedalSolution? medalSolution = await Context.MedalSolutions.FirstOrDefaultAsync(e => e.MedalId == medalId);
        return medalSolution?.ToDto();
    }
}