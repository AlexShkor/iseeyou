using System.Collections.Generic;

namespace ISeeYou
{
    public class SourceAnalyzer
    {
        private readonly string _sourceId;
        private readonly string[] _subjects;

        public SourceAnalyzer(string sourceId, string[] subjects)
        {
            _sourceId = sourceId;
            _subjects = subjects;
        }

        public void Run()
        {
            var photos = GetPhotos(_sourceId);
            foreach (var photo in photos)
            {
                var photoAnalyzer = new EntityAnalyzer(_sourceId,"photo", photo, _subjects);
            }
        }

        private IEnumerable<int> GetPhotos(object sourceId)
        {
            throw new System.NotImplementedException();
        }
    }

    public class EntityAnalyzer
    {
        public EntityAnalyzer(string sourceId, string entityType, int entityId, string[] subjects)
        {
            throw new System.NotImplementedException();
        }
    }
}