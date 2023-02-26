using System.Text.Json.Serialization;

namespace Osekai.Octon.Persistence.EntityFramework.MySql.Serializables;

public struct SerializableSessionPayload
{
    [JsonConstructor]
    public SerializableSessionPayload(string osuApiV2Token, string osuApiV2RefreshToken, int osuUserId, DateTime expiresAt)
    {
        OsuApiV2Token = osuApiV2Token;
        OsuApiV2RefreshToken = osuApiV2RefreshToken;
        OsuUserId = osuUserId;
        ExpiresAt = expiresAt;
    }

    public string OsuApiV2Token { get; }
    public string OsuApiV2RefreshToken { get; }
    public int OsuUserId { get; }
    public DateTime ExpiresAt { get; }
}