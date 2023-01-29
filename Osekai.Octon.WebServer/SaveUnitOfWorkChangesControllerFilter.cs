using Microsoft.AspNetCore.Mvc.Filters;
using Osekai.Octon.Persistence;

namespace Osekai.Octon.WebServer;

public class SaveUnitOfWorkChangesControllerFilter: IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        IUnitOfWork unitOfWork = context.HttpContext.RequestServices.GetRequiredService<IUnitOfWork>();
        ActionExecutedContext executedContext = await next.Invoke();
        
        if (executedContext.Exception == null)
            await unitOfWork.SaveChangesAsync(context.HttpContext.RequestAborted);
    }
}