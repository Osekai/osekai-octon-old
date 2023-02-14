using Osekai.Octon.Models;
using Osekai.Octon.Persistence;

namespace Osekai.Octon.RichModels.Extensions;

public static class ReadOnlyAppExtension
{
    public static App ToRichModel(this IReadOnlyApp app, IUnitOfWork unitOfWork)
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