using Microsoft.AspNetCore.Mvc;
using Osekai.Octon.Domain.Aggregates;
using Osekai.Octon.Services;
using Osekai.Octon.WebServer.API.V1.Dtos.AppFaqController;

namespace Osekai.Octon.WebServer.API.V1;

public sealed class AppFaqController: Controller
{
    private readonly AppService _appService;
    private readonly IAdapter<App, AppWithFaqDto> _appWithFaqDtoFromAppAdapter;
    
    public AppFaqController(AppService appService, IAdapter<App, AppWithFaqDto> appWithFaqDtoFromAppAdapter)
    {
        _appWithFaqDtoFromAppAdapter = appWithFaqDtoFromAppAdapter;
        _appService = appService;
    }

    [HttpGet("/api/v1/faqs")]
    [HttpGet("/home/api/faq.php")]
    public async Task<IActionResult> GetFaqsAsync(CancellationToken cancellationToken)
    {
        IEnumerable<App> apps = await _appService.GetAppsAsync(includeFaqs: true, cancellationToken);
        return Ok(await apps.ToAsyncEnumerable()
            .SelectAwait(async a => await _appWithFaqDtoFromAppAdapter.AdaptAsync(a, cancellationToken))
            .ToArrayAsync(cancellationToken));
    }
}