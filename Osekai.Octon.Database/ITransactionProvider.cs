﻿using System.Data;

namespace Osekai.Octon.Database;

public interface ITransactionProvider
{
    Task<ITransaction> BeginTransactionAsync(
        IsolationLevel isolationLevel = IsolationLevel.Serializable,
        CancellationToken cancellationToken = default);
}