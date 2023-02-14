using Osekai.Octon.Models;
using Osekai.Octon.Persistence;

namespace Osekai.Octon.RichModels.Extensions;

public static class ReadOnlySessionExtension
{
    public static Session ToRichModel(this IReadOnlySession session, IUnitOfWork unitOfWork)
    {
        return new Session(
            session.Token,
            session.Payload,
            session.ExpiresAt,
            unitOfWork);
    }
}