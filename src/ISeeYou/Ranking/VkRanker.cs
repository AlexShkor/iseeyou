﻿using System.Collections.Generic;
using System.Linq;
using ISeeYou.Documents;
using ISeeYou.ViewServices;
using ISeeYou.Vk.Api;
using ISeeYou.Vk.Dto;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace ISeeYou
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

        public List<RankedProfile> GetRankedProfiles(string screenName)
        {
            return null;
        }

        public List<RankedProfile> UpdateRankedProfiles(int id)
        {
            var api = new VkApi(null);
            var profile = api.GetUsers(new[] { id.ToString() }, new[] { "sex", "relatives", "university", "schools" }).FirstOrDefault();
            var friends = api.GetUserFriends(id.ToString(), new[] { "sex", "university", "schools" });

            if (profile != null)
            {
                RankBySex(profile, friends);
                //RankByCommonFriends(profile, friends);
                //RankByRelatives(profile);
                //RankBySchoolAndUniversity(profile, friends);
            }

            return _ranks.Select(x => new RankedProfile(x.Key.Value, x.Value, id)).ToList();
        }

        //private void RankByCommonFriends(VkUser subject, IEnumerable<FriendDto> friends)
        //{
        //    foreach (var friend in friends)
        //    {
        //        if (friend.Id.HasValue)
        //        {
        //            if (subject.Id != null)
        //            {
        //                try
        //                {
        //                    var commonFriends = Friends.GetMutual(subject.Id.Value, friend.Id).Result;
        //                    _ranks[friend.Id] += commonFriends.Count;
        //                    _sources.Items.Update(Query<SourceDocument>.EQ(x => x.Id, friend.Id), Update<SourceDocument>.Inc(x => x.Rank, commonFriends.Count).Set(x => x.SubjectId, subject.Id), UpdateFlags.Upsert);
        //                }
        //                catch (Exception ex)
        //                {
        //                    continue;
        //                }
        //            }
        //        }
        //    }
            
        //}

        //private void RankBySchoolAndUniversity(User subject, IEnumerable<User> friends)
        //{
        //    var targetUniversity = subject.University;
        //    var targetSchools = subject.Schools;

        //    var friendsUniversity =
        //        friends.Where(x => x.University != null && targetUniversity != null && x.University == targetUniversity);
        //    var friendsSchools =
        //        friends.Where(
        //            x =>
        //                x.Schools != null && targetSchools != null &&
        //                x.Schools.Select(s => s.Id).Intersect(targetSchools.Select(ts => ts.Id)).Any());
        //    var combinedEducationFriends = friendsUniversity.Concat(friendsSchools).Distinct();
        //    foreach (var friend in combinedEducationFriends)
        //    {
        //        _ranks[friend.Id] += RANK_STEP;
        //        _sources.Items.Update(Query<SourceDocument>.EQ(x => x.Id, friend.Id), Update<SourceDocument>.Inc(x => x.Rank, RANK_STEP).Set(x => x.SubjectId, subject.Id), UpdateFlags.Upsert);
        //    }
        //}

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

        private int GetRankBySex(string targetSex, string userSex)
        {
            if (targetSex == null)
            {
                return 0;
            }
            if (targetSex == "male")
            {
                if (userSex == "Female")
                {
                    return RANK_STEP;
                }
                return 0;
            }
            else
            {
                if (userSex == "male")
                {
                    return RANK_STEP;
                }
                return 0;
            }
        }

    }
}