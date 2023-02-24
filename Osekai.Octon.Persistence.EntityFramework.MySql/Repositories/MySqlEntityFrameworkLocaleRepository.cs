using Osekai.Octon.Domain.AggregateRoots;
using Osekai.Octon.Domain.Repositories;

namespace Osekai.Octon.Persistence.EntityFramework.MySql.Repositories;

public class MySqlEntityFrameworkLocaleRepository: ILocaleRepository
{
    protected MySqlOsekaiDbContext Context { get; }
    
    public MySqlEntityFrameworkLocaleRepository(MySqlOsekaiDbContext context)
    {
        Context = context;
    }
    
    public async Task<IEnumerable<Locale>> GetLocalesAsync(CancellationToken cancellationToken = default) =>
        await Context.Locales.ToAsyncEnumerable().Select(s => s.ToAggregateRoot()).ToArrayAsync(cancellationToken);
}