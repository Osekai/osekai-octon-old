using Osekai.Octon.Models;
using Osekai.Octon.Persistence.Repositories;

namespace Osekai.Octon.Persistence.EntityFramework.MySql.Repositories;

public class MySqlEntityFrameworkLocaleRepository: ILocaleRepository
{
    protected MySqlOsekaiDbContext Context { get; }
    
    public MySqlEntityFrameworkLocaleRepository(MySqlOsekaiDbContext context)
    {
        Context = context;
    }
    
    public async Task<IEnumerable<IReadOnlyLocale>> GetLocalesAsync(CancellationToken cancellationToken = default) =>
        await Context.Locales.ToAsyncEnumerable().Select(s => s.ToDto()).ToArrayAsync(cancellationToken);
}