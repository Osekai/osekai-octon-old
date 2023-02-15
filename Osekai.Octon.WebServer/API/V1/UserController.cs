using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Osekai.Octon.OsuApi;
using Osekai.Octon.OsuApi.Payloads;
using Osekai.Octon.WebServer.API.V1.Dtos.UserController;

namespace Osekai.Octon.WebServer.API.V1;

[DefaultAuthenticationFilter]
public sealed class UserController: Controller
{
    private readonly IAuthenticatedOsuApiV2Interface _osuApiV2Interface;
    private readonly CurrentSession _currentSession;
    
    public UserController(CurrentSession currentSession, IAuthenticatedOsuApiV2Interface osuApiV2Interface)
    {
        _osuApiV2Interface = osuApiV2Interface;
        _currentSession = currentSession;
    }
    
    [HttpGet("/api/v1/users/{userId:int}")]
    [HttpGet("/api/profiles/get_user.php")]
    public async Task<IActionResult> GetUserByIdAsync(
        [FromQuery(Name = "id")] int? queryUserId, int userId,
        CancellationToken cancellationToken)
    {
        if (queryUserId != null)
            userId = queryUserId.Value;
        
        OsuUser? osuUser = await _osuApiV2Interface.SearchUserAsync(_currentSession, userId.ToString(), cancellationToken: cancellationToken);

        if (osuUser == null)
            return NotFound();

        UserDto osuUserDto = new UserDto();
        osuUserDto.AvatarUrl = osuUser.AvatarUrl;
        osuUserDto.CountryCode = osuUser.CountryCode;
        osuUserDto.DefaultGroup = osuUser.DefaultGroup;
        osuUserDto.Id = osuUser.Id;
        osuUserDto.IsActive = osuUser.IsActive;
        osuUserDto.IsBot = osuUser.IsBot;
        osuUserDto.IsDeleted = osuUser.IsDeleted;
        osuUserDto.IsOnline = osuUser.IsOnline;
        osuUserDto.IsSupporter = osuUser.IsSupporter;
        osuUserDto.LastVisit = osuUser.LastVisit;
        osuUserDto.PmFriendsOnly = osuUser.PmFriendsOnly;
        osuUserDto.ProfileColour = osuUser.ProfileColour;
        osuUserDto.Username = osuUser.Username;
        osuUserDto.CoverUrl = osuUser.CoverUrl;
        osuUserDto.Discord = osuUser.Discord;
        osuUserDto.HasSupported = osuUser.HasSupported;
        
        osuUserDto.Interests = osuUser.Interests;
        osuUserDto.JoinDate = osuUser.JoinDate;

        osuUserDto.Kudosu = new UserDtoKudosu
        {
            Available = osuUser.Kudosu.Available,
            Total = osuUser.Kudosu.Total
        };

        osuUserDto.Location = osuUser.Location;
        osuUserDto.MaxBlocks = osuUser.MaxBlocks;
        osuUserDto.MaxFriends = osuUser.MaxFriends;
        
        osuUserDto.Occupation = osuUser.Occupation;
        osuUserDto.Playmode = osuUser.Playmode;
        osuUserDto.Playstyle = osuUser.Playstyle;
        osuUserDto.PostCount = osuUser.PostCount;
        osuUserDto.ProfileOrder = osuUser.ProfileOrder;
        
        osuUserDto.Title = osuUser.Title;
        osuUserDto.Twitter = osuUser.Twitter;
        osuUserDto.Website = osuUser.Website;
        
        osuUserDto.Country = new UserDtoCountry
        {
            Code = osuUser.Country.Code,
            Name = osuUser.Country.Name
        };

        osuUserDto.Cover = new UserDtoCover
        {
            Id = osuUser.Cover.Id,
            Url = osuUser.Cover.Url,
            CustomUrl = osuUser.Cover.CustomUrl
        };

        osuUserDto.IsRestricted = osuUser.IsRestricted;
        osuUserDto.AccountHistory = osuUser.AccountHistory;
        osuUserDto.ActiveTournamentBanner = osuUser.ActiveTournamentBanner;
        
        osuUserDto.Badges = new UserDtoBadge[osuUser.Badges.Length];
        for(int x = 0; x < osuUser.Badges.Length; x++)
        {
            var badge = new UserDtoBadge
            {
                Description = osuUser.Badges[x].Description,
                Url = osuUser.Badges[x].Url,
                AwardedAt = osuUser.Badges[x].AwardedAt,
                ImageUrl = osuUser.Badges[x].ImageUrl
            };
            osuUserDto.Badges[x] = badge;
        }
        
        osuUserDto.FavouriteBeatmapsetCount = osuUser.FavouriteBeatmapsetCount;
        osuUserDto.FollowerCount = osuUser.FollowerCount;
        
        osuUserDto.GraveyardBeatmapsetCount = osuUser.GraveyardBeatmapsetCount;
        
        osuUserDto.Groups = new UserDtoGroup[osuUser.Groups.Length];
        for (int x = 0; x < osuUser.Groups.Length; x++)
        {
            var group = new UserDtoGroup
            {
                Id = osuUser.Groups[x].Id,
                Identifier = osuUser.Groups[x].Identifier,
                Name = osuUser.Groups[x].Name,
                ShortName = osuUser.Groups[x].ShortName,
                Description = osuUser.Groups[x].Description,
                Colour = osuUser.Groups[x].Colour
            };
            osuUserDto.Groups[x] = group;
        }

        osuUserDto.LovedBeatmapsetCount = osuUser.LovedBeatmapsetCount;
        osuUserDto.MonthlyPlaycounts = new UserDtoPlaycount[osuUser.MonthlyPlaycounts.Length];
        for (int x = 0; x < osuUser.MonthlyPlaycounts.Length; x++)
        {
            var playcount = new UserDtoPlaycount()
            {
                Count = osuUser.MonthlyPlaycounts[x].Count,
                StartDate = osuUser.MonthlyPlaycounts[x].StartDate
            };
            osuUserDto.MonthlyPlaycounts[x] = playcount;
        }
        
        osuUserDto.Page = new UserDtoPage
        {
            Html = osuUser.Page.Html,
            Raw = osuUser.Page.Raw
        };

        osuUserDto.PendingBeatmapsetCount = osuUser.PendingBeatmapsetCount;
        
        osuUserDto.PreviousUsernames = osuUser.PreviousUsernames;
        osuUserDto.RankedBeatmapsetCount = osuUser.RankedBeatmapsetCount;
        
        osuUserDto.ReplaysWatchedCounts = new UserDtoPlaycount[osuUser.ReplaysWatchedCounts.Length];
        for (int x = 0; x < osuUser.ReplaysWatchedCounts.Length; x++)
        {
            var playcount = new UserDtoPlaycount()
            {
                Count = osuUser.ReplaysWatchedCounts[x].Count,
                StartDate = osuUser.ReplaysWatchedCounts[x].StartDate
            };
            osuUserDto.ReplaysWatchedCounts[x] = playcount;
        }
        
        osuUserDto.ScoresFirstCount = osuUser.ScoresFirstCount;
        
        osuUserDto.Statistics = new UserDtoStatistics
        {
            Pp = osuUser.Statistics.Pp,
            GlobalRank = osuUser.Statistics.GlobalRank,
            RankedScore = osuUser.Statistics.RankedScore,
            HitAccuracy = osuUser.Statistics.HitAccuracy,
            PlayCount = osuUser.Statistics.PlayCount,
            PlayTime = osuUser.Statistics.PlayTime,
            TotalScore = osuUser.Statistics.TotalScore,
            TotalHits = osuUser.Statistics.TotalHits,
            MaximumCombo = osuUser.Statistics.MaximumCombo,
            ReplaysWatchedByOthers = osuUser.Statistics.ReplaysWatchedByOthers,
            IsRanked = osuUser.Statistics.IsRanked,
        };
        osuUserDto.Statistics.Level = new UserDtoLevel
        {
            Current = osuUser.Statistics.Level.Current,
            Progress = osuUser.Statistics.Level.Progress
        };
        osuUserDto.Statistics.GradeCounts = new UserDtoGradeCounts
        {
            Ss = osuUser.Statistics.GradeCounts.Ss,
            Ssh = osuUser.Statistics.GradeCounts.Ssh,
            S = osuUser.Statistics.GradeCounts.S,
            Sh = osuUser.Statistics.GradeCounts.Sh,
            A = osuUser.Statistics.GradeCounts.A
        };
        osuUserDto.Statistics.Rank = new UserDtoRank
        {
            Global = osuUser.Statistics.Rank.Global,
            Country = osuUser.Statistics.Rank.Country
        };
        
        osuUserDto.SupportLevel = osuUser.SupportLevel;

        osuUserDto.UserAchievements = new UserDtoUserAchievement[osuUser.UserAchievements.Length];
        for (int x = 0; x < osuUser.UserAchievements.Length; x++)
        {
            var achievement = new UserDtoUserAchievement()
            {
                AchievedAt = osuUser.UserAchievements[x].AchievedAt,
                AchievementId = osuUser.UserAchievements[x].AchievementId
            };
            osuUserDto.UserAchievements[x] = achievement;
        }

        osuUserDto.RankHistory = new UserDtoRankHistory()
        {
            Data = osuUser.RankHistory.Data,
            Mode = osuUser.RankHistory.Mode
        };

        // needs to be converted to a DTO
        // has been converted to a DTO :3
        return Ok(osuUserDto);
    }
}