using System.Text.Json.Serialization;

namespace Osekai.Octon.WebServer.Presentation.AppBaseLayout;

public class AppBaseLayoutUserGroupData
{
    [JsonPropertyName("Id")]
    public int Id { get; set; }

    [JsonPropertyName("Name")]
    public string Name { get; set; } = null!;
    
    [JsonPropertyName("ShortName")]
    public string ShortName { get; set; } = null!;
    
    [JsonPropertyName("Description")]
    public string Description { get; set; } = null!;
    
    [JsonPropertyName("Colour")]
    public string Colour { get; set; } = null!;
    
    [JsonPropertyName("Order")]
    public int Order { get; set; }
    
    [JsonPropertyName("Hidden")]
    public bool Hidden { get; set; }
    
    [JsonPropertyName("ForceVisible")]
    public bool ForceVisible { get; set; }
}