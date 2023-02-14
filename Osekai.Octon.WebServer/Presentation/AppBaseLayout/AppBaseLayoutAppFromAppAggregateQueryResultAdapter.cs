using System.Drawing;
using System.Globalization;
using System.Text;
using Microsoft.Extensions.ObjectPool;
using Osekai.Octon.Persistence.QueryResults;

namespace Osekai.Octon.WebServer.Presentation.AppBaseLayout;

public class AppBaseLayoutAppFromAppAggregateQueryResultAdapter: IAdapter<IReadOnlyAppAggregateQueryResult, AppBaseLayoutApp>
{
    protected ObjectPool<StringBuilder> StringBuilderObjectPool { get; }

    protected CurrentLocale CurrentLocale { get; }
    
    public AppBaseLayoutAppFromAppAggregateQueryResultAdapter(CurrentLocale currentLocale,
        ObjectPool<StringBuilder> stringBuilderObjectPool)
    {
        CurrentLocale = currentLocale;
        StringBuilderObjectPool = stringBuilderObjectPool;
    }
    
    private string GetColourString(Color color)
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
    
    public async Task<AppBaseLayoutApp> AdaptAsync(IReadOnlyAppAggregateQueryResult e, CancellationToken cancellationToken = default)
    {
        return new AppBaseLayoutApp
        {
            Color = GetColourString(e.AppTheme?.Color ?? Color.FromArgb(53, 61, 85)),
            Cover = GetCoverString(e.App.SimpleName),
            Experimental = e.App.Experimental ? "1" : "0",
            Id = e.App.Name.ToString(),
            ColourLogo = GetColouredLogo(e.App.SimpleName),
            Logo = GetLogo(e.App.SimpleName),
            Name = e.App.Name,
            SimpleName = e.App.SimpleName,
            Order = e.App.Order.ToString(),
            Slogan = await CurrentLocale.Localizator.GetStringAsync($"apps.{e.App.SimpleName}.slogan"),
            Visible = e.App.Visible ? "1" : "0",
            ColorDark = GetColourString(e.AppTheme?.DarkColor ?? Color.FromArgb(53, 61, 85)),
            HasCover = (e.AppTheme?.HasCover ?? false) ? "1" : "0",
            ValueMultiplier = (e.AppTheme?.HslValueMultiplier ?? 1f).ToString(CultureInfo.InvariantCulture),
            DarkValueMultiplier = (e.AppTheme?.DarkHslValueMultiplier ?? 1f).ToString(CultureInfo.InvariantCulture)
        };
    }
}