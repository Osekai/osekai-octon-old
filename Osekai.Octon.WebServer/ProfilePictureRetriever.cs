using Osekai.Octon.OsuApi.Payloads;

namespace Osekai.Octon.WebServer;

public static class ProfilePictureRetriever
{
    public static string? GetProfilePictureFromOsuUser(OsuUser? osuUser) =>
        osuUser == null ? "https://osu.ppy.sh/assets/images/avatar-guest.8a2df920.png" : osuUser.AvatarUrl.ToString();
}