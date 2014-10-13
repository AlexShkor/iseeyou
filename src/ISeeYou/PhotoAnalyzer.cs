using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ISeeYou.Domain.Aggregates.Subject.Commands;
using ISeeYou.Vk.Api;

namespace ISeeYou
{
    public class PhotoAnalyzer
    {
        private readonly int _sourceId;
        private readonly WallPost _wallPost;
        private readonly List<int> _subjects;

        public PhotoAnalyzer(int sourceId, WallPost wallPost, List<int> subjects)
        {
            _sourceId = sourceId;
            _subjects = subjects;
            _wallPost = wallPost;
        }

        public void Run()
        {
            var api = new VkApi(null);
            var result = api.Likes(_wallPost.id, _sourceId);
            if (result != null && result.Any())
            {
                var intersect = result.Intersect(_subjects);
                var photo = _wallPost.attachments.Select(x => x.photo).FirstOrDefault();
                foreach (var subjectId in intersect)
                {
                    GlobalQueue.Send(new AddPhotoLike
                    {
                        Id = subjectId.ToString(CultureInfo.InvariantCulture),
                        SubjectId = subjectId,
                        StartDate = UnixTimeStampToDateTime(_wallPost.date),
                        EndDate = DateTime.UtcNow,
                        PhotoId = _wallPost.id,
                        SourceId = _sourceId,
                        Image = photo.photo_130,
                        ImageBig = photo.photo_604
                    });
                }
            }
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}