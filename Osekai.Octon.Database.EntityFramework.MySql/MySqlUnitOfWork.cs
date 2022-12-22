using Osekai.Octon.Database.EntityFramework.Repositories;
using Osekai.Octon.Database.Repositories;

namespace Osekai.Octon.Database.EntityFramework;

public class MySqlUnitOfWork: EntityFrameworkUnitOfWork<MySqlOsekaiDbContext>
{
    public MySqlUnitOfWork(MySqlOsekaiDbContext context) : base(context)
    {
        
    }

    private IAppRepository? _appRepository;

    public override IAppRepository AppRepository => _appRepository ??= new MySqlEntityFrameworkAppRepository(Context);
}