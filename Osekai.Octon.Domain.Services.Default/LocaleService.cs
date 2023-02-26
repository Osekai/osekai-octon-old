using Osekai.Octon.Domain.AggregateRoots;
using Osekai.Octon.Domain.Services;
using Osekai.Octon.Persistence;

namespace Osekai.Octon.Domain.Services.Default;

public class LocaleService : ILocaleService
{
    protected IUnitOfWork UnitOfWork { get; }
    
    public LocaleService(IUnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork;
    }

    public Task<IEnumerable<Locale>> GetLocalesAsync(CancellationToken cancellationToken = default) =>
        UnitOfWork.LocaleRepository.GetLocalesAsync(cancellationToken);
}