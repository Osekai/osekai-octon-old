using Osekai.Octon.Models;
using Osekai.Octon.Persistence;

namespace Osekai.Octon.Services;

public class LocaleService
{
    protected IUnitOfWork UnitOfWork { get; }
    
    public LocaleService(IUnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork;
    }

    public Task<IEnumerable<IReadOnlyLocale>> GetLocalesAsync(CancellationToken cancellationToken = default) =>
        UnitOfWork.LocaleRepository.GetLocalesAsync(cancellationToken);
}