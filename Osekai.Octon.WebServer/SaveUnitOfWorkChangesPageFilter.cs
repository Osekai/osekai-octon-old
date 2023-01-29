using Microsoft.AspNetCore.Mvc.Filters;
using Osekai.Octon.Persistence;

namespace Osekai.Octon.WebServer;

public class SaveUnitOfWorkChangesPageFilter: IAsyncPageFilter
{
    public Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
    {
        return Task.CompletedTask;
    }

    public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
    {
        IUnitOfWork unitOfWork = context.HttpContext.RequestServices.GetRequiredService<IUnitOfWork>();
        PageHandlerExecutedContext executedContext = await next.Invoke();
        
        if (executedContext.Exception == null)
            await unitOfWork.SaveChangesAsync(context.HttpContext.RequestAborted);
    }
}