using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ISeeYou.Domain.Aggregates.Subject.Commands;
using ISeeYou.Vk.Api;

namespace ISeeYou
{
    public class SourceAnalyzer
    {
        private readonly int _sourceId;
        private readonly List<int> _subjects;

        public SourceAnalyzer(int sourceId, List<int> subjects)
        {
            _sourceId = sourceId;
            _subjects = subjects;
        }

        public void Run()
        {
            var api = new VkApi(null);
            var albums = api.GetAlbums(_sourceId).Select(x => x.aid.ToString(CultureInfo.InvariantCulture)).Concat(new[] { "profile", "wall" });
            foreach (var album in albums)
            {
                var photos = api.GetPhotos(_sourceId, album);
                foreach (var photoDto in photos)
                {
                    var result = api.Likes(photoDto.pid, _sourceId);
                    if (result != null && result.Any())
                    {
                        var intersect = result.Intersect(_subjects);
                        foreach (var subjectId in intersect)
                        {
                            GlobalQueue.Send(new AddPhotoLike
                            {
                                Id = subjectId.ToString(CultureInfo.InvariantCulture),
                                SubjectId = subjectId,
                                StartDate = UnixTimeStampToDateTime(photoDto.created),
                                EndDate = DateTime.UtcNow,
                                PhotoId = photoDto.pid,
                                SourceId = _sourceId,
                                Image = photoDto.src,
                                ImageBig = photoDto.src_big
                            });
                        }
                    }
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