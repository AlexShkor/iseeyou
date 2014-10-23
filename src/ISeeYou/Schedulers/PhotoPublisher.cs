using System;
using ISeeYou.MQ;
using ISeeYou.MQ.Events;

namespace ISeeYou.Schedulers
{
    public class PhotoPublisher
    {
        private RabbitMqPublisher _publisher;

        public PhotoPublisher(SiteSettings settings)
        {
            _publisher = new RabbitMqPublisher(settings.RabbitHost, settings.RabbitUser, settings.RabbitPwd, settings.PhotosQueue);
        }

        public void Publish(int photoId, int ownerId, string docId, bool newSource = false, int? subjectId = null)
        {
            _publisher.Publish(new PhotoFetchEvent
            {
                Payload = new PhotoFetchPayload
                {
                    UserId = ownerId,
                    PhotoId = photoId,
                    DocId = docId,
                    NewSource = newSource,
                    SubjectId = subjectId,
                    Published = DateTime.UtcNow
                }
            });
        }
    }
}