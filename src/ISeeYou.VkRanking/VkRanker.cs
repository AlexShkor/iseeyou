using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ISeeYou.Documents;
using ISeeYou.Helpers;
using ISeeYou.ViewServices;
using ISeeYou.VkRanking.Models;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using VkAPIAsync;
using VkAPIAsync.Authorization;
using VkAPIAsync.Wrappers.Friends;
using VkAPIAsync.Wrappers.Users;

namespace ISeeYou.VkRanking
{
    public class VkRanker
    {
        private const int RANK_STEP = 10;
        private readonly SourcesViewService _sources;
        private Dictionary<int?, int> _ranks;

        public VkRanker(SourcesViewService sources)
        {
            _ranks = new Dictionary<int?, int>();
            _sources = sources;

        }

        public void Authorize(int appId, string email, string password)
        {
            VkAPI.AppId = appId;
            var auth = new HiddenAuth();
            auth.Auth(email, password);
        }

        public List<RankedProfile> GetRankedProfiles(string screenName)
        {
            return null;
        }

        public List<RankedProfile> UpdateRankedProfiles(int id)
        {
            var profile = Users.Get(new[] { id.ToString(CultureInfo.InvariantCulture) }, new[] { "sex", "relatives", "university", "schools" }).Result.FirstOrDefault();
            Thread.Sleep(400);
            Console.WriteLine("Sleep " + 400);
            var friends = Friends.Get(id, new[] { "sex", "university", "schools" }).Result;
            Console.WriteLine("Profile " +(profile != null));
            if (profile != null)
            {
                RankBySex(profile, friends);
                RankByCommonFriends(profile, friends);
                RankByRelatives(profile);
                RankBySchoolAndUniversity(profile, friends);
            }

            return _ranks.Select(x => new RankedProfile(x.Key.Value, x.Value, id)).ToList();
        }

        private void RankByCommonFriends(User subject, IEnumerable<User> friends)
        {
            foreach (var friend in friends)
            {
                if (friend.Id.HasValue)
                {
                    if (subject.Id != null)
                    {
                        try
                        {
                            Thread.Sleep(400);
                            var commonFriends = Friends.GetMutual(subject.Id.Value, friend.Id).Result;
                            _ranks[friend.Id] += commonFriends.Count;
                            _sources.Items.Update(Query<SourceDocument>.EQ(x => x.Id, friend.Id), Update<SourceDocument>.Inc(x => x.Rank, commonFriends.Count).Set(x => x.SubjectId, subject.Id), UpdateFlags.Upsert);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error... RankByCommonFriends");
                        }
                    }
                }
            }
            
        }

        private void RankBySchoolAndUniversity(User subject, IEnumerable<User> friends)
        {
            var targetUniversity = subject.University;
            var targetSchools = subject.Schools;

            var friendsUniversity =
                friends.Where(x => x.University != null && targetUniversity != null && x.University == targetUniversity);
            var friendsSchools =
                friends.Where(
                    x =>
                        x.Schools != null && targetSchools != null &&
                        x.Schools.Select(s => s.Id).Intersect(targetSchools.Select(ts => ts.Id)).Any());
            var combinedEducationFriends = friendsUniversity.Concat(friendsSchools).Distinct();
            foreach (var friend in combinedEducationFriends)
            {
                _ranks[friend.Id] += RANK_STEP;
                _sources.Items.Update(Query<SourceDocument>.EQ(x => x.Id, friend.Id), Update<SourceDocument>.Inc(x => x.Rank, RANK_STEP).Set(x => x.SubjectId, subject.Id), UpdateFlags.Upsert);
            }
        }

        private void RankByRelatives(User subject)
        {
            var relatives = subject.Relatives;
            if (relatives != null)
            {
                foreach (var relative in relatives)
                {
                    if (_ranks.Keys.Contains(relative.Id))
                    {
                        _ranks[relative.Id] += RANK_STEP;
                        _sources.Items.Update(Query<SourceDocument>.EQ(x => x.Id, relative.Id), Update<SourceDocument>.Inc(x => x.Rank, RANK_STEP).Set(x => x.SubjectId, subject.Id), UpdateFlags.Upsert);
                    }
                    else
                    {
                        _ranks.Add(relative.Id, RANK_STEP);
                        _sources.Items.Insert(new SourceDocument() {Id = relative.Id.Value, Rank = RANK_STEP, SubjectId = subject.Id.Value});
                    }
                    
                }
            }
        }

        private void RankBySex(User subject, IEnumerable<User> friends)
        {
            foreach (var friend in friends)
            {
                var rank = GetRankBySex(friend.Sex, subject.Sex);
                _ranks.Add(friend.Id, rank);
                _sources.Items.Update(Query<SourceDocument>.EQ(x => x.Id, friend.Id), Update<SourceDocument>.Inc(x => x.Rank, rank).Set(x => x.SubjectId, subject.Id), UpdateFlags.Upsert);
            }
        }

        private int GetRankBySex(UserSex.UserSexEnum targetSex, UserSex.UserSexEnum userSex)
        {
            if (targetSex == UserSex.UserSexEnum.Unknown)
            {
                return 0;
            }
            if (targetSex == UserSex.UserSexEnum.Male)
            {
                if (userSex == UserSex.UserSexEnum.Female)
                {
                    return RANK_STEP;
                }
                return 0;
            }
            else
            {
                if (userSex == UserSex.UserSexEnum.Male)
                {
                    return RANK_STEP;
                }
                return 0;
            }
        }

    }
}
