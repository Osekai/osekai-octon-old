namespace Osekai.Octon.Options;

public class OsuOAuthConfiguration
{
    public int ClientId { get; set; } = 0;
    public string ClientSecret { get; set; } = string.Empty;
    public string RedirectUri { get; set; } = string.Empty;
}