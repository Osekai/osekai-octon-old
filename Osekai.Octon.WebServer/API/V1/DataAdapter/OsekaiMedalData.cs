using System.Text.Json.Serialization;

namespace Osekai.Octon.WebServer.API.V1.DataAdapter;

#nullable disable

public class OsekaiMedalData
{
    [JsonPropertyName("MedalID")]
    public long MedalId { get; set; }

    [JsonPropertyName("Name")]
    public string Name { get; set; }

    [JsonPropertyName("Link")]
    public Uri Link { get; set; }

    [JsonPropertyName("Description")]
    public string Description { get; set; }

    [JsonPropertyName("Restriction")]
    public string Restriction { get; set; }

    [JsonPropertyName("Grouping")]
    public string Grouping { get; set; }

    [JsonPropertyName("Instructions")]
    public string Instructions { get; set; }

    [JsonPropertyName("Solution")]
    public string Solution { get; set; }

    [JsonPropertyName("Mods")]
    public string Mods { get; set; }

    [JsonPropertyName("Locked")]
    public long Locked { get; set; }

    [JsonPropertyName("Video")]
    public object Video { get; set; }

    [JsonPropertyName("Date")]
    public string Date { get; set; }

    [JsonPropertyName("PackID")]
    public object PackId { get; set; }

    [JsonPropertyName("FirstAchievedDate")]
    public object FirstAchievedDate { get; set; }

    [JsonPropertyName("FirstAchievedBy")]
    public object FirstAchievedBy { get; set; }

    [JsonPropertyName("ModeOrder")]
    public long ModeOrder { get; set; }

    [JsonPropertyName("Ordering")]
    public long Ordering { get; set; }

    [JsonPropertyName("Rarity")]
    public double Rarity { get; set; }
}