using Osekai.Octon.Persistence;
using Osekai.Octon.Persistence.Dtos;
using Osekai.Octon.Services.Entities;

namespace Osekai.Octon.Services.Extensions;

internal static class MedalDtoExtension
{
    internal static Medal ToEntity(this MedalDto medalDto, IUnitOfWork unitOfWork)
    {
        return new Medal(
            medalDto.Id,
            medalDto.Name,
            medalDto.Link,
            medalDto.Description,
            medalDto.Restriction,
            medalDto.Grouping,
            medalDto.Instructions,
            medalDto.Ordering,
            medalDto.Video,
            medalDto.Date,
            medalDto.FirstAchievedDate,
            medalDto.FirstAchievedBy,
            medalDto.Rarity,
            medalDto.TimesOwned,
            unitOfWork
        );
    }
}