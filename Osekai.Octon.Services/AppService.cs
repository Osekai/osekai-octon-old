﻿using Osekai.Octon.Domain.Aggregates;
using Osekai.Octon.Persistence;

namespace Osekai.Octon.Services;

public class AppService
{
    protected IUnitOfWork UnitOfWork { get; }
    
    public AppService(IUnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork;
    }

    public Task<App?> GetAppByIdAsync(int id, bool includeFaqs = false, CancellationToken cancellationToken = default)
        => UnitOfWork.AppRepository.GetAppByIdAsync(id, includeFaqs, cancellationToken);

    public Task<IEnumerable<App>> GetAppsAsync(bool includeFaqs = false, CancellationToken cancellationToken = default) =>
        UnitOfWork.AppRepository.GetAppsAsync(includeFaqs, cancellationToken);
}