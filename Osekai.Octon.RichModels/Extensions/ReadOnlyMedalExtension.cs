using Osekai.Octon.Models;
using Osekai.Octon.Persistence;

namespace Osekai.Octon.RichModels.Extensions;

internal static class ReadOnlyMedalExtension
{
    public static Medal ToRichModel(this IReadOnlyMedal medal, IUnitOfWork unitOfWork)
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