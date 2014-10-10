using System.Collections.Generic;
using VkAPIAsync.Wrappers.Common;
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
                if (photo.Id.HasValue)
                {
                    var photoAnalyzer = new PhotoAnalyzer(_sourceId, photo, _subjects);
                    photoAnalyzer.Run();
                }
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
}