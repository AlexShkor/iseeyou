using System;
using System.Collections.Generic;
using System.Linq;
using ISeeYou.Documents;
using ISeeYou.ViewServices;
using ISeeYou.Vk.Api;
using ISeeYou.Vk.Dto;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace ISeeYou.VkRanking
{
    public class VkRanker
    {
        private const int RANK_STEP = 10;
        private readonly SourcesViewService _sources;
        private Dictionary<int?, int> _ranks;
        private readonly VkApi _api;

        public VkRanker(SourcesViewService sources)
        {
            _ranks = new Dictionary<int?, int>();
            _sources = sources;
            var api = new VkApi(null);
        }

        public List<RankedProfile> GetRankedProfiles(string screenName)
        {
            return null;
        }

        public void UpdateRankedProfiles(int id)
        {
            
            var fields = new[] { "sex", "education", "city", "bdate", "lists", "followers_count" };
            var profile = _api.GetUsers(new[] { id.ToString() }, fields).FirstOrDefault();
            var friends = _api.GetUserFriends(id.ToString(), fields);

            if (profile != null)
            {
                foreach (var friend in friends)
                {
                    _sources.Items.Update(Query.And(Query<SourceDocument>.EQ(x => x.Id, friend.UserId), Query<SourceDocument>.EQ(x => x.SubjectId, id)), Update<SourceDocument>.Inc(x => x.Rank, 50).Set(x => x.SubjectId, id), UpdateFlags.Upsert);
                }
                RankBySex(profile, friends);
                RankByCommonFriends(profile, friends);
                //RankByRelatives(profile);
                //RankBySchoolAndUniversity(profile, friends);
            }
        }

        private void RankByCommonFriends(VkUser subject, IEnumerable<VkUser> friends)
        {
            foreach (var friend in friends)
            {
                try
                {
                    var commonFriends = _api.GetMutualFriends(subject.UserId.ToString(), friend.UserId.ToString());
                    _ranks[friend.UserId] += commonFriends.Count();
                    _sources.Items.Update(Query<SourceDocument>.EQ(x => x.Id, friend.UserId), Update<SourceDocument>.Inc(x => x.Rank, commonFriends.Count()).Set(x => x.SubjectId, subject.UserId), UpdateFlags.Upsert);
                }
                catch (Exception ex)
                {
                    continue;
                }
            }
        }

        private void RankBySchoolAndUniversity(VkUser subject, IEnumerable<VkUser> friends)
        {
            var targetUniversity = subject.education.university;
            //var targetSchools = subject.Schools;

            var friendsUniversity =
                friends.Where(x => x.education.university != null && targetUniversity != null && x.education.university == targetUniversity);
            //var friendsSchools =
            //    friends.Where(
            //        x =>
            //            x.Schools != null && targetSchools != null &&
            //            x.Schools.Select(s => s.Id).Intersect(targetSchools.Select(ts => ts.Id)).Any());
            var combinedEducationFriends = friendsUniversity;//.Concat(friendsSchools).Distinct();
            foreach (var friend in combinedEducationFriends)
            {
                _ranks[friend.UserId] += RANK_STEP;
                _sources.Items.Update(Query<SourceDocument>.EQ(x => x.Id, friend.UserId), Update<SourceDocument>.Inc(x => x.Rank, RANK_STEP).Set(x => x.SubjectId, subject.UserId), UpdateFlags.Upsert);
            }
        }

        //private void RankByRelatives(VkUser subject)
        //{
        //    var relatives = subject.Relatives;
        //    if (relatives != null)
        //    {
        //        foreach (var relative in relatives)
        //        {
        //            if (_ranks.Keys.Contains(relative.Id))
        //            {
        //                _ranks[relative.Id] += RANK_STEP;
        //                _sources.Items.Update(Query<SourceDocument>.EQ(x => x.Id, relative.Id), Update<SourceDocument>.Inc(x => x.Rank, RANK_STEP).Set(x => x.SubjectId, subject.Id), UpdateFlags.Upsert);
        //            }
        //            else
        //            {
        //                _ranks.Add(relative.Id, RANK_STEP);
        //                _sources.Items.Insert(new SourceDocument() {Id = relative.Id.Value, Rank = RANK_STEP, SubjectId = subject.Id.Value});
        //            }

        //        }
        //    }
        //}

        private void RankBySex(VkUser subject, IEnumerable<VkUser> friends)
        {
            foreach (var friend in friends)
            {
                var rank = GetRankBySex(friend.Sex, subject.Sex);
                _ranks.Add(friend.UserId, rank);
                _sources.Items.Update(Query<SourceDocument>.EQ(x => x.Id, friend.UserId), Update<SourceDocument>.Inc(x => x.Rank, rank).Set(x => x.SubjectId, subject.UserId), UpdateFlags.Upsert);
            }
        }

        private int GetRankBySex(Sex targetSex, Sex userSex)
        {
            if (targetSex == Sex.None)
            {
                return 0;
            }
            if (targetSex == Sex.Male)
            {
                if (userSex == Sex.Female)
                {
                    return RANK_STEP;
                }
                return 0;
            }
            else
            {
                if (userSex == Sex.Male)
                {
                    return RANK_STEP;
                }
                return 0;
            }
        }

    }
}
