using System.Text.Json.Serialization;

namespace Osekai.Octon.WebServer.API.V1.Dtos.UserController;

#nullable disable

public class UserDto
{
    [JsonPropertyName("avatar_url")]
    public Uri AvatarUrl { get; set; }

    [JsonPropertyName("country_code")]
    public string CountryCode { get; set; }

    [JsonPropertyName("default_group")]
    public string DefaultGroup { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("is_active")]
    public bool IsActive { get; set; }

    [JsonPropertyName("is_bot")]
    public bool IsBot { get; set; }

    [JsonPropertyName("is_deleted")]
    public bool IsDeleted { get; set; }

    [JsonPropertyName("is_online")]
    public bool IsOnline { get; set; }

    [JsonPropertyName("is_supporter")]
    public bool IsSupporter { get; set; }

    [JsonPropertyName("last_visit")]
    public DateTimeOffset? LastVisit { get; set; }

    [JsonPropertyName("pm_friends_only")]
    public bool PmFriendsOnly { get; set; }

    [JsonPropertyName("profile_colour")]
    public string ProfileColour { get; set; }

    [JsonPropertyName("username")]
    public string Username { get; set; }

    [JsonPropertyName("cover_url")]
    public Uri CoverUrl { get; set; }

    [JsonPropertyName("discord")]
    public string Discord { get; set; }

    [JsonPropertyName("has_supported")]
    public bool HasSupported { get; set; }

    [JsonPropertyName("interests")]
    public object Interests { get; set; }

    [JsonPropertyName("join_date")]
    public DateTimeOffset JoinDate { get; set; }

    [JsonPropertyName("kudosu")]
    public Kudosu Kudosu { get; set; }

    [JsonPropertyName("location")]
    public object Location { get; set; }

    [JsonPropertyName("max_blocks")]
    public long MaxBlocks { get; set; }

    [JsonPropertyName("max_friends")]
    public long MaxFriends { get; set; }

    [JsonPropertyName("occupation")]
    public object Occupation { get; set; }

    [JsonPropertyName("playmode")]
    public string Playmode { get; set; }

    [JsonPropertyName("playstyle")]
    public string[] Playstyle { get; set; }

    [JsonPropertyName("post_count")]
    public long PostCount { get; set; }

    [JsonPropertyName("profile_order")]
    public string[] ProfileOrder { get; set; }

    [JsonPropertyName("title")]
    public object Title { get; set; }

    [JsonPropertyName("twitter")]
    public string Twitter { get; set; }

    [JsonPropertyName("website")]
    public Uri Website { get; set; }

    [JsonPropertyName("country")]
    public UserDtoCountry Country { get; set; }

    [JsonPropertyName("cover")]
    public UserDtoCover Cover { get; set; }

    [JsonPropertyName("is_restricted")]
    public bool IsRestricted { get; set; }

    [JsonPropertyName("account_history")]
    public object[] AccountHistory { get; set; }

    [JsonPropertyName("active_tournament_banner")]
    public object ActiveTournamentBanner { get; set; }

    [JsonPropertyName("badges")]
    public UserDtoBadge[] Badges { get; set; }

    [JsonPropertyName("favourite_beatmapset_count")]
    public long FavouriteBeatmapsetCount { get; set; }

    [JsonPropertyName("follower_count")]
    public long FollowerCount { get; set; }

    [JsonPropertyName("graveyard_beatmapset_count")]
    public long GraveyardBeatmapsetCount { get; set; }

    [JsonPropertyName("groups")]
    public UserDtoGroup[] Groups { get; set; }

    [JsonPropertyName("loved_beatmapset_count")]
    public long LovedBeatmapsetCount { get; set; }

    [JsonPropertyName("monthly_playcounts")]
    public UserDtoPlaycount[] MonthlyPlaycounts { get; set; }

    [JsonPropertyName("page")]
    public UserDtoPage Page { get; set; }

    [JsonPropertyName("pending_beatmapset_count")]
    public long PendingBeatmapsetCount { get; set; }

    [JsonPropertyName("previous_usernames")]
    public object[] PreviousUsernames { get; set; }

    [JsonPropertyName("ranked_beatmapset_count")]
    public long RankedBeatmapsetCount { get; set; }

    [JsonPropertyName("replays_watched_counts")]
    public UserDtoPlaycount[] ReplaysWatchedCounts { get; set; }

    [JsonPropertyName("scores_first_count")]
    public long ScoresFirstCount { get; set; }

    [JsonPropertyName("statistics")]
    public UserDtoStatistics Statistics { get; set; }

    [JsonPropertyName("support_level")]
    public long SupportLevel { get; set; }

    [JsonPropertyName("user_achievements")]
    public UserDtoUserAchievement[] UserAchievements { get; set; }

    [JsonPropertyName("rank_history")]
    public UserDtoRankHistory RankHistory { get; set; }
}


public partial class UserDtoBadge
{
    [JsonPropertyName("awarded_at")]
    public DateTimeOffset AwardedAt { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("image_url")]
    public Uri ImageUrl { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }
}

public partial class UserDtoCountry
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}

public partial class UserDtoCover
{
    [JsonPropertyName("custom_url")]
    public Uri CustomUrl { get; set; }

    [JsonPropertyName("url")]
    public Uri Url { get; set; }

    [JsonPropertyName("id")]
    public object Id { get; set; }
}

public partial class UserDtoGroup
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("identifier")]
    public string Identifier { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("short_name")]
    public string ShortName { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("colour")]
    public string Colour { get; set; }
}

public partial class Kudosu
{
    [JsonPropertyName("total")]
    public long Total { get; set; }

    [JsonPropertyName("available")]
    public long Available { get; set; }
}

public class UserDtoPlaycount
{
    [JsonPropertyName("start_date")]
    public DateTimeOffset StartDate { get; set; }

    [JsonPropertyName("count")]
    public long Count { get; set; }
}

public partial class UserDtoPage
{
    [JsonPropertyName("html")]
    public string Html { get; set; }

    [JsonPropertyName("raw")]
    public string Raw { get; set; }
}

public partial class UserDtoRankHistory
{
    [JsonPropertyName("mode")]
    public string Mode { get; set; }

    [JsonPropertyName("data")]
    public long[] Data { get; set; }
}

public partial class UserDtoStatistics
{
    [JsonPropertyName("level")]
    public Level Level { get; set; }

    [JsonPropertyName("pp")]
    public double Pp { get; set; }

    [JsonPropertyName("global_rank")]
    public long? GlobalRank { get; set; }

    [JsonPropertyName("ranked_score")]
    public long RankedScore { get; set; }

    [JsonPropertyName("hit_accuracy")]
    public double HitAccuracy { get; set; }

    [JsonPropertyName("play_count")]
    public long PlayCount { get; set; }

    [JsonPropertyName("play_time")]
    public long PlayTime { get; set; }

    [JsonPropertyName("total_score")]
    public long TotalScore { get; set; }

    [JsonPropertyName("total_hits")]
    public long TotalHits { get; set; }

    [JsonPropertyName("maximum_combo")]
    public long MaximumCombo { get; set; }

    [JsonPropertyName("replays_watched_by_others")]
    public long ReplaysWatchedByOthers { get; set; }

    [JsonPropertyName("is_ranked")]
    public bool IsRanked { get; set; }

    [JsonPropertyName("grade_counts")]
    public GradeCounts GradeCounts { get; set; }

    [JsonPropertyName("rank")]
    public Rank Rank { get; set; }
}

public partial class GradeCounts
{
    [JsonPropertyName("ss")]
    public long Ss { get; set; }

    [JsonPropertyName("ssh")]
    public long Ssh { get; set; }

    [JsonPropertyName("s")]
    public long S { get; set; }

    [JsonPropertyName("sh")]
    public long Sh { get; set; }

    [JsonPropertyName("a")]
    public long A { get; set; }
}

public partial class Level
{
    [JsonPropertyName("current")]
    public long Current { get; set; }

    [JsonPropertyName("progress")]
    public long Progress { get; set; }
}

public partial class Rank
{
    [JsonPropertyName("global")]
    public long? Global { get; set; }

    [JsonPropertyName("country")]
    public long? Country { get; set; }
}

public partial class UserDtoUserAchievement
{
    [JsonPropertyName("achieved_at")]
    public DateTimeOffset AchievedAt { get; set; }

    [JsonPropertyName("achievement_id")]
    public long AchievementId { get; set; }
}
