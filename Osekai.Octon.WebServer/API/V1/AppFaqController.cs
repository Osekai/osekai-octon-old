using Microsoft.AspNetCore.Mvc;
using Osekai.Octon.Domain.AggregateRoots;
using Osekai.Octon.Domain.Services;
using Osekai.Octon.WebServer.API.V1.Dtos.AppFaqController;

namespace Osekai.Octon.WebServer.API.V1;

public sealed class AppFaqController: Controller
{
    private readonly IAppService _appService;
    private readonly IConverter<App, AppWithFaqDto> _appWithFaqDtoFromAppConverter;
    
    public AppFaqController(IAppService appService, IConverter<App, AppWithFaqDto> appWithFaqDtoFromAppConverter)
    {
        _appWithFaqDtoFromAppConverter = appWithFaqDtoFromAppConverter;
        _appService = appService;
    }

    [HttpGet("/api/v1/faqs")]
    [HttpGet("/home/api/faq.php")]
    public async Task<IActionResult> GetFaqsAsync(CancellationToken cancellationToken)
    {
        IEnumerable<App> apps = await _appService.GetAppsAsync(includeFaqs: true, cancellationToken);
        return Ok(await apps.ToAsyncEnumerable()
            .SelectAwait(async a => await _appWithFaqDtoFromAppConverter.AdaptAsync(a, cancellationToken))
            .ToArrayAsync(cancellationToken));
    }
}