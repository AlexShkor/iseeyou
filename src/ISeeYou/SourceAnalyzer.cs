using System;
using System.Collections.Generic;
using System.Linq;
using VkAPIAsync.Wrappers.Common;
using VkAPIAsync.Wrappers.Likes;
using VkAPIAsync.Wrappers.Photos;

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

        public async void Run()
        {
            var photos = GetPhotos(_sourceId);
            foreach (var photo in photos)
            {
                var photoAnalyzer = new PhotoAnalyzer(_sourceId,photo, _subjects);
                photoAnalyzer.Run();
            }
        }

        private IEnumerable<Photo> GetPhotos(int sourceId)
        {
            var offset = 0;
            const int count = 200;
            ListCount<Photo> result = null;
            do
            {
                result = Photos.GetAll(sourceId, count, offset).Result;
                foreach (var photo in result)
                {
                    yield return photo;
                }
                offset += count;
            } while (result.TotalCount > offset + count);

        }
    }

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

        public async void Run()
        {
            var offset = 0;
            const int count = 200;
            ListCount<int> result = null;
            do
            {
                result = await Likes.GetList(new LikeType(LikeType.LikeTypeEnum.Photo), _sourceId, _photo.Id, offset: 0, count: count);
                var intersect = result.Intersect(_subjects);
                foreach (var subjectId in intersect)
                {
                    GlobalQueue.Send(new AddPhotoLike
                    {
                        SubjectId = subjectId,
                        StartDate = _photo.DateCreated,
                        EndDate = DateTime.UtcNow,
                        PhotoId = _photo.Id,
                        SourceId = _sourceId,
                        Image = _photo.Photo130
                    });
                }
                offset += count;
            } while (result.TotalCount > offset + count);
        }
    }

    public class AddPhotoLike
    {
        public int SubjectId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? PhotoId { get; set; }
        public int SourceId { get; set; }
        public string Image { get; set; }
    }
}