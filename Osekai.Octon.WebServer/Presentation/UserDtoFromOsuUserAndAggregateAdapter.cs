using Osekai.Octon.Domain.AggregateRoots;
using Osekai.Octon.OsuApi.Payloads;
using Osekai.Octon.WebServer.API.V1.Dtos.UserController;

namespace Osekai.Octon.WebServer.Presentation;

public class UserDtoFromOsuUserAndAggregateAdapter: IAdapter<(OsuUser, IEnumerable<UserGroup>), UserDto>
{
    public ValueTask<UserDto> AdaptAsync((OsuUser, IEnumerable<UserGroup>) value, CancellationToken cancellationToken = default)
    {
        var (osuUser, userGroups) = value;
        
        UserDto userDto = new UserDto();
        userDto.AvatarUrl = osuUser.AvatarUrl;
        userDto.CountryCode = osuUser.CountryCode;
        userDto.DefaultGroup = osuUser.DefaultGroup;
        userDto.Id = osuUser.Id;
        userDto.IsActive = osuUser.IsActive;
        userDto.IsBot = osuUser.IsBot;
        userDto.IsDeleted = osuUser.IsDeleted;
        userDto.IsOnline = osuUser.IsOnline;
        userDto.IsSupporter = osuUser.IsSupporter;
        userDto.LastVisit = osuUser.LastVisit;
        userDto.PmFriendsOnly = osuUser.PmFriendsOnly;
        userDto.ProfileColour = osuUser.ProfileColour;
        userDto.Username = osuUser.Username;
        userDto.CoverUrl = osuUser.CoverUrl;
        userDto.Discord = osuUser.Discord;
        userDto.HasSupported = osuUser.HasSupported;
        
        userDto.Interests = osuUser.Interests;
        userDto.JoinDate = osuUser.JoinDate;

        userDto.Kudosu = new UserDtoKudosu
        {
            Available = osuUser.Kudosu.Available,
            Total = osuUser.Kudosu.Total
        };

        userDto.Location = osuUser.Location;
        userDto.MaxBlocks = osuUser.MaxBlocks;
        userDto.MaxFriends = osuUser.MaxFriends;
        
        userDto.Occupation = osuUser.Occupation;
        userDto.Playmode = osuUser.Playmode;
        userDto.Playstyle = osuUser.Playstyle;
        userDto.PostCount = osuUser.PostCount;
        userDto.ProfileOrder = osuUser.ProfileOrder;
        
        userDto.Title = osuUser.Title;
        userDto.Twitter = osuUser.Twitter;
        userDto.Website = osuUser.Website;
        
        userDto.Country = new UserDtoCountry
        {
            Code = osuUser.Country.Code,
            Name = osuUser.Country.Name
        };

        userDto.Cover = new UserDtoCover
        {
            Id = osuUser.Cover.Id,
            Url = osuUser.Cover.Url,
            CustomUrl = osuUser.Cover.CustomUrl
        };

        userDto.IsRestricted = osuUser.IsRestricted;
        userDto.AccountHistory = osuUser.AccountHistory;
        userDto.ActiveTournamentBanner = osuUser.ActiveTournamentBanner;
        
        userDto.Badges = new UserDtoBadge[osuUser.Badges.Length];
        for(int x = 0; x < osuUser.Badges.Length; x++)
        {
            var badge = new UserDtoBadge
            {
                Description = osuUser.Badges[x].Description,
                Url = osuUser.Badges[x].Url,
                AwardedAt = osuUser.Badges[x].AwardedAt,
                ImageUrl = osuUser.Badges[x].ImageUrl
            };
            userDto.Badges[x] = badge;
        }
        
        userDto.FavouriteBeatmapsetCount = osuUser.FavouriteBeatmapsetCount;
        userDto.FollowerCount = osuUser.FollowerCount;
        
        userDto.GraveyardBeatmapsetCount = osuUser.GraveyardBeatmapsetCount;
        
        userDto.Groups = new UserDtoGroup[osuUser.Groups.Length];
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
            userDto.Groups[x] = group;
        }

        userDto.LovedBeatmapsetCount = osuUser.LovedBeatmapsetCount;
        userDto.MonthlyPlaycounts = new UserDtoPlaycount[osuUser.MonthlyPlaycounts.Length];
        for (int x = 0; x < osuUser.MonthlyPlaycounts.Length; x++)
        {
            var playcount = new UserDtoPlaycount()
            {
                Count = osuUser.MonthlyPlaycounts[x].Count,
                StartDate = osuUser.MonthlyPlaycounts[x].StartDate
            };
            userDto.MonthlyPlaycounts[x] = playcount;
        }
        
        userDto.Page = new UserDtoPage
        {
            Html = osuUser.Page.Html,
            Raw = osuUser.Page.Raw
        };

        userDto.PendingBeatmapsetCount = osuUser.PendingBeatmapsetCount;
        
        userDto.PreviousUsernames = osuUser.PreviousUsernames;
        userDto.RankedBeatmapsetCount = osuUser.RankedBeatmapsetCount;
        
        userDto.ReplaysWatchedCounts = new UserDtoPlaycount[osuUser.ReplaysWatchedCounts.Length];
        for (int x = 0; x < osuUser.ReplaysWatchedCounts.Length; x++)
        {
            var playcount = new UserDtoPlaycount()
            {
                Count = osuUser.ReplaysWatchedCounts[x].Count,
                StartDate = osuUser.ReplaysWatchedCounts[x].StartDate
            };
            userDto.ReplaysWatchedCounts[x] = playcount;
        }
        
        userDto.ScoresFirstCount = osuUser.ScoresFirstCount;
        
        userDto.Statistics = new UserDtoStatistics
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
        userDto.Statistics.Level = new UserDtoLevel
        {
            Current = osuUser.Statistics.Level.Current,
            Progress = osuUser.Statistics.Level.Progress
        };
        userDto.Statistics.GradeCounts = new UserDtoGradeCounts
        {
            Ss = osuUser.Statistics.GradeCounts.Ss,
            Ssh = osuUser.Statistics.GradeCounts.Ssh,
            S = osuUser.Statistics.GradeCounts.S,
            Sh = osuUser.Statistics.GradeCounts.Sh,
            A = osuUser.Statistics.GradeCounts.A
        };
        userDto.Statistics.Rank = new UserDtoRank
        {
            Global = osuUser.Statistics.Rank.Global,
            Country = osuUser.Statistics.Rank.Country
        };
        
        userDto.SupportLevel = osuUser.SupportLevel;

        userDto.UserAchievements = new UserDtoUserAchievement[osuUser.UserAchievements.Length];
        for (int x = 0; x < osuUser.UserAchievements.Length; x++)
        {
            var achievement = new UserDtoUserAchievement()
            {
                AchievedAt = osuUser.UserAchievements[x].AchievedAt,
                AchievementId = osuUser.UserAchievements[x].AchievementId
            };
            userDto.UserAchievements[x] = achievement;
        }

        userDto.RankHistory = new UserDtoRankHistory()
        {
            Data = osuUser.RankHistory.Data,
            Mode = osuUser.RankHistory.Mode
        };

        userDto.UserGroups = userGroups.Select(e => new UserDtoUserGroup(osuUser.Id, e.Id)).ToArray();
        
        return ValueTask.FromResult(userDto);
    }
}