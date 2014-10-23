using System;
using ISeeYou.MQ;
using ISeeYou.MQ.Events;

namespace ISeeYou.Schedulers
{
    public class SourcePublisher
    {
        private RabbitMqPublisher _publisher;

        public SourcePublisher(SiteSettings settings)
        {
            _publisher = new RabbitMqPublisher(settings.RabbitHost, settings.RabbitUser, settings.RabbitPwd, settings.SourcesQueue);
        }

        public void Publish(int sourceId, int? subjectId = null)
        {
            _publisher.Publish(new SourceFetchEvent
            {
                Payload = new SourceFetchPayload()
                {
                    UserId = sourceId,
                    Published = DateTime.UtcNow,
                    SubjectId = subjectId
                }
            });
        }
    }
}