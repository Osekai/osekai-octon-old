using Osekai.Octon.Objects;
using Osekai.Octon.Persistence;
using Osekai.Octon.Services.Entities;

namespace Osekai.Octon.Services.Extensions;

internal static class ReadOnlyMedalExtension
{
    internal static Medal ToEntity(this IReadOnlyMedal medal, IUnitOfWork unitOfWork)
    {
        return new Medal(
            medal.Id,
            medal.Name,
            medal.Link,
            medal.Description,
            medal.Restriction,
            medal.Grouping,
            medal.Instructions,
            medal.Ordering,
            medal.Video,
            medal.Date,
            medal.FirstAchievedDate,
            medal.FirstAchievedBy,
            medal.Rarity,
            medal.TimesOwned,
            unitOfWork
        );
    }
}