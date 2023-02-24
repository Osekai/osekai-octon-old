using System.Text.Json.Serialization;

namespace Osekai.Octon.WebServer.API.V1.Dtos.TeamMemberController;

public class TeamMemberDto
{
    [JsonPropertyName("id")]
    public int UserId { get; init; }
    
    [JsonPropertyName("name")]
    public string Name { get; init; } = null!;
    
    [JsonPropertyName("name_alt")]
    public string? NameAlt { get; init; }
    
    [JsonPropertyName("role")]
    public string Role { get; init; } = null!;
    
    [JsonPropertyName("groups")]
    public IReadOnlyList<int> UserGroupIds { get; init; } = null!;
    
    [JsonPropertyName("socials")]
    public IReadOnlyList<TeamMemberDtoSocial> Socials { get; init; } = null!;
}

public readonly struct TeamMemberDtoSocial
{
    public string Icon { get; init; }
    public string Link { get; init; }
    public string Name { get; init; }
}