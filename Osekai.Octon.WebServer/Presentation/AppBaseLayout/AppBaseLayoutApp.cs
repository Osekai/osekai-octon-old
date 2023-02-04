using System.Text.Json.Serialization;

namespace Osekai.Octon.WebServer.Presentation.AppBaseLayout;

public sealed class AppBaseLayoutApp
{
    [JsonPropertyName("id")]
    public string Id { get; init; }

    [JsonPropertyName("order")]
    public string Order { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; }

    [JsonPropertyName("slogan")]
    public string Slogan { get; init; }

    [JsonPropertyName("simplename")]
    public string SimpleName { get; init; }

    [JsonPropertyName("color_dark")]
    public string ColorDark { get; init; }

    [JsonPropertyName("color")]
    public string Color { get; init; }

    [JsonPropertyName("logo")]
    public string Logo { get; init; }

    [JsonPropertyName("colour_logo")]
    public string ColourLogo { get; init; }

    [JsonPropertyName("cover")]
    public string Cover { get; init; }

    [JsonPropertyName("visible")]
    public string Visible { get; init; }

    [JsonPropertyName("experimental")]
    public string Experimental { get; init; }

    [JsonPropertyName("hascover")]
    public string HasCover { get; init; }

    [JsonPropertyName("dark_value_multiplier")]
    public string DarkValueMultiplier { get; init; }

    [JsonPropertyName("value_mulitplier")]
    public string ValueMultiplier { get; init; }
}