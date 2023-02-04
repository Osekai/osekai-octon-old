using Osekai.Octon.Objects;
using Osekai.Octon.Persistence;
using Osekai.Octon.Services.Entities;

namespace Osekai.Octon.Services.Extensions;

internal static class ReadOnlyAppExtension
{
    internal static App ToEntity(this IReadOnlyApp app, IUnitOfWork unitOfWork)
    {
        return new App(
            app.Id, 
            app.Order, 
            app.Name, 
            app.SimpleName,
            app.Visible,
            app.Experimental,
            unitOfWork
        );
    }
}