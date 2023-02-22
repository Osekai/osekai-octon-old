using System.Text.Json.Serialization;

namespace Osekai.Octon.WebServer.API.V1.Dtos.AppFaqController;

public class AppWithFaqDto
{
    [JsonPropertyName("id")]
    public string Id { get; init; } = null!;

    [JsonPropertyName("order")]
    public string Order { get; init; } = null!;

    [JsonPropertyName("name")]
    public string Name { get; init; } = null!;
 
    [JsonPropertyName("slogan")]
    public string Slogan { get; init; } = null!;
 
    [JsonPropertyName("simplename")]
    public string SimpleName { get; init; } = null!;

    [JsonPropertyName("color_dark")]
    public string ColorDark { get; init; } = null!;

    [JsonPropertyName("color")]
    public string Color { get; init; } = null!;

    [JsonPropertyName("logo")]
    public string Logo { get; init; } = null!;

    [JsonPropertyName("colour_logo")]
    public string ColourLogo { get; init; } = null!;

    [JsonPropertyName("cover")]
    public string Cover { get; init; } = null!;

    [JsonPropertyName("visible")]
    public string Visible { get; init; } = null!;

    [JsonPropertyName("experimental")]
    public string Experimental { get; init; } = null!;

    [JsonPropertyName("hascover")]
    public string HasCover { get; init; } = null!;

    [JsonPropertyName("dark_value_multiplier")]
    public string DarkValueMultiplier { get; init; } = null!;

    [JsonPropertyName("value_mulitplier")]
    public string ValueMultiplier { get; init; } = null!;

    [JsonPropertyName("questions")] 
    public IReadOnlyList<FaqDto> Faqs { get; init; } = null!;
}

public readonly struct FaqDto
{
    [JsonPropertyName("ID")] 
    public int Id { get; init; }
    
    [JsonPropertyName("App")]
    public int AppId { get; init; }
    
    [JsonPropertyName("Title")]
    public string Title { get; init; }

    [JsonPropertyName("Content")]
    public string Content { get; init; }
    
    [JsonPropertyName("LocalizationPrefix")]
    public string LocalizationPrefix { get; init; } 
}