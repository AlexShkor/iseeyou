using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISeeYou.Vk.Api;
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

        public void Run()
        {
            var wallPosts = GetWall(_sourceId).ToList();
            foreach (var wallPost in wallPosts)
            {
                    var photoAnalyzer = new PhotoAnalyzer(_sourceId, wallPost, _subjects);
                    photoAnalyzer.Run();
            }
        }

        private IEnumerable<WallPost> GetWall(int sourceId)
        {
            var api = new VkApi(null);
            return api.GetWall(sourceId).Where(x=> x.attachments != null && x.attachments.Any(a => a.photo != null));

        }
    }
}