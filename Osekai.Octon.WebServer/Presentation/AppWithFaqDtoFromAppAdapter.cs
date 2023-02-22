using System.Globalization;
using System.Text;
using Microsoft.Extensions.ObjectPool;
using Osekai.Octon.Domain.Aggregates;
using Osekai.Octon.Drawing;
using Osekai.Octon.WebServer.API.V1.Dtos.AppFaqController;

namespace Osekai.Octon.WebServer.Presentation;

public class AppWithFaqDtoFromAppAdapter: IAdapter<App, AppWithFaqDto>
{ 
    protected ObjectPool<StringBuilder> StringBuilderObjectPool { get; }

    protected CurrentLocale CurrentLocale { get; }
    
    public AppWithFaqDtoFromAppAdapter(CurrentLocale currentLocale,
        ObjectPool<StringBuilder> stringBuilderObjectPool)
    {
        CurrentLocale = currentLocale;
        StringBuilderObjectPool = stringBuilderObjectPool;
    }
    
    private string GetColourString(RgbColour color)
    {
        StringBuilder stringBuilder = StringBuilderObjectPool.Get();
        try
        {
            stringBuilder.Append($"{color.R},{color.B},{color.G}");
            return stringBuilder.ToString();
        }
        finally
        {
            StringBuilderObjectPool.Return(stringBuilder);
        }
    }

    private string GetCoverString(string name)
    {
        switch (name)
        {
            case "home":
                return "cover/none";
            default:
                return $"cover/{name}";
        }
    }

    private string GetLogo(string name)
    {
        switch (name)
        {
            case "home":
                return "osekai_light";
            default:
                return $"white/{name}";
        }
    }
    
    private string GetColouredLogo(string name)
    {
        switch (name)
        {
            case "home":
                return "osekai_dark";
            default:
                return $"coloured/{name}";
        }
    }

    public async Task<AppWithFaqDto> AdaptAsync(App e, CancellationToken cancellationToken = default)
    {
        return new AppWithFaqDto
        {
            Color = GetColourString(e.AppTheme.Value?.Color ?? new RgbColour(53, 61, 85)),
            Cover = GetCoverString(e.SimpleName),
            Experimental = e.Experimental ? "1" : "0",
            Id = e.Name.ToString(),
            ColourLogo = GetColouredLogo(e.SimpleName),
            Logo = GetLogo(e.SimpleName),
            Name = e.Name,
            SimpleName = e.SimpleName,
            Order = e.Order.ToString(),
            Slogan = await CurrentLocale.Localizator.GetStringAsync($"apps.{e.SimpleName}.slogan"),
            Visible = e.Visible ? "1" : "0",
            ColorDark = GetColourString(e.AppTheme.Value?.DarkColor ?? new RgbColour(53, 61, 85)),
            HasCover = (e.AppTheme.Value?.HasCover ?? false) ? "1" : "0",
            ValueMultiplier = (e.AppTheme.Value?.HslValueMultiplier ?? 1f).ToString(CultureInfo.InvariantCulture),
            DarkValueMultiplier = (e.AppTheme.Value?.DarkHslValueMultiplier ?? 1f).ToString(CultureInfo.InvariantCulture),
            Faqs = e.Faqs.Value.Select(f => new FaqDto
            {
                Id = f.Id,
                Content = f.Content,
                Title = f.Title,
                AppId = f.AppId,
                LocalizationPrefix = f.LocalizationPrefix
            }).ToArray()
        };
    }
}