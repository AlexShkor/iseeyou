using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISeeYou.VkRanking.Models;
using VkAPIAsync;
using VkAPIAsync.Authorization;
using VkAPIAsync.Wrappers.Friends;
using VkAPIAsync.Wrappers.Users;

namespace ISeeYou.VkRanking
{
    public class VkRanker
    {
        private const int RANK_STEP = 10;

        private readonly int _appId;
        private readonly string _email;
        private readonly string _password;

        private Dictionary<int?, int> _ranks;

        public VkRanker(int appId, string email, string password)
        {
            VkAPI.AppId = appId;
            _email = email;
            _password = password;
            _ranks = new Dictionary<int?, int>();

        }

        public void Authorize()
        {
            var auth = new HiddenAuth();
            auth.Auth(_email, _password);
        }

        public List<RankedProfile> GetRankedProfiles(string screenName)
        {
            return null;
        }

        public List<RankedProfile> GetRankedProfiles(int id)
        {
            var profile = Users.Get(new[] { id.ToString() }, new[] { "sex", "relatives", "university", "schools" }).Result.FirstOrDefault();
            var friends = Friends.Get(id, new[] { "sex", "university", "schools" }).Result;

            if (profile != null)
            {
                RankBySex(profile.Sex, friends);
                RankByCommonFriends(profile, friends);
                RankByRelatives(profile);
                RankBySchoolAndUniversity(profile, friends);
            }
            return _ranks.Select(x => new RankedProfile(x.Key.Value, x.Value)).OrderByDescending(x => x.Rank).ToList();
        }

        private void RankByCommonFriends(User target, IEnumerable<User> friends)
        {
            var commonFriendsWithUser = new Dictionary<int?, int>();
            try
            {

                foreach (var friend in friends)
                {
                    if (friend.Id.HasValue)
                    {
                        if (target.Id != null)
                        {
                            var commonFriends = Friends.GetMutual(target.Id.Value, friend.Id).Result;
                            commonFriendsWithUser.Add(friend.Id, commonFriends.Count);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return;
            }
            finally
            {
                foreach (var i in commonFriendsWithUser)
                {
                    _ranks[i.Key] += i.Value;
                }
            }

        }

        private void RankBySchoolAndUniversity(User target, IEnumerable<User> friends)
        {
            var targetUniversity = target.University;
            var targetSchools = target.Schools;

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
            }
        }

        private void RankByRelatives(User target)
        {
            var relatives = target.Relatives;
            if (relatives != null)
            {
                foreach (var relative in relatives)
                {
                    if (_ranks.Keys.Contains(relative.Id))
                    {
                        _ranks[relative.Id] += RANK_STEP;
                    }
                    else
                    {
                        _ranks.Add(relative.Id, RANK_STEP);
                    }
                }
            }
        }

        private void RankBySex(UserSex.UserSexEnum targeSex, IEnumerable<User> friends)
        {
            foreach (var friend in friends)
            {
                _ranks.Add(friend.Id, GetRankBySex(friend.Sex, targeSex));
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
