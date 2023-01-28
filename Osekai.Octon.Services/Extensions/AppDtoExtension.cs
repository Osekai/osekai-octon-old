using Osekai.Octon.Persistence;
using Osekai.Octon.Persistence.Dtos;
using Osekai.Octon.Persistence.Repositories;
using Osekai.Octon.Services.Entities;

namespace Osekai.Octon.Services.Extensions;

internal static class AppDtoExtension
{
    internal static App ToEntity(this AppDto appDto, IDatabaseUnitOfWork unitOfWork)
    {
        return new App(
            appDto.Id, 
            appDto.Order, 
            appDto.Name, 
            appDto.SimpleName,
            appDto.Visible,
            appDto.Experimental,
            unitOfWork
        );
    }
}