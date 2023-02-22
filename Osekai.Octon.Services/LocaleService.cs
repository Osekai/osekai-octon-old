using Osekai.Octon.Domain.Aggregates;
using Osekai.Octon.Domain.Entities;
using Osekai.Octon.Persistence;

namespace Osekai.Octon.Services;

public class LocaleService
{
    protected IUnitOfWork UnitOfWork { get; }
    
    public LocaleService(IUnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork;
    }

    public Task<IEnumerable<Locale>> GetLocalesAsync(CancellationToken cancellationToken = default) =>
        UnitOfWork.LocaleRepository.GetLocalesAsync(cancellationToken);
}