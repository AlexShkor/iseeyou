using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ISeeYou.Domain.Aggregates.Subject.Commands;
using VkAPIAsync.Wrappers.Common;
using VkAPIAsync.Wrappers.Likes;
using VkAPIAsync.Wrappers.Photos;

namespace ISeeYou
{
    public class PhotoAnalyzer
    {
        private readonly int _sourceId;
        private readonly Photo _photo;
        private readonly List<int> _subjects;

        public PhotoAnalyzer(int sourceId, Photo photo, List<int> subjects)
        {
            _sourceId = sourceId;
            _subjects = subjects;
            _photo = photo;
        }

        public async Task Run()
        {
            var offset = 0;
            const int count = 200;
            ListCount<int> result = null;
            do
            {
                await Task.Delay(300);
                result = await Likes.GetList(new LikeType(LikeType.LikeTypeEnum.Photo), _sourceId, _photo.Id, offset: 0, count: count);
                if (result != null && result.Any())
                {
                    var intersect = result.Intersect(_subjects);
                    foreach (var subjectId in intersect)
                    {
                        GlobalQueue.Send(new AddPhotoLike
                        {
                            Id = subjectId.ToString(CultureInfo.InvariantCulture),
                            SubjectId = subjectId,
                            StartDate = _photo.DateCreated,
                            EndDate = DateTime.UtcNow,
                            PhotoId = _photo.Id.Value,
                            SourceId = _sourceId,
                            Image = _photo.Photo130
                        });
                    }
                }
                offset += count;
            } while (result != null && result.TotalCount > offset);
        }
    }
}