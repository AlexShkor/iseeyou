using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISeeYou.Domain.Aggregates.Subject.Commands;
using ISeeYou.ViewServices;
using ISeeYou.Vk.Api;
using StructureMap;

namespace ISeeYou.Worker
{
    class Program
    {

        static void Main(string[] args)
        {
            var container = ObjectFactory.Container;
            new Bootstrapper().ConfigureSettings(container);
            new Bootstrapper().ConfigureMongoDb(container);
            var consumer = container.GetInstance<IConsumer>();
            var api = new VkApi();
            while (true)
            {
                var subjects = container.GetInstance<SubjectViewService>().GetAll().Select(x => x.Id).ToList();
                for (int i = 0; i < 100; i++)
                {
                    var photo = consumer.Pull();
                    var result = api.Likes(photo.Id, photo.UserId);
                    if (result != null && result.Any())
                    {
                        var intersect = result.Intersect(subjects);
                        PhotoDocument doc = null;
                        foreach (var subjectId in intersect)
                        {
                            if (doc == null)
                            {
                                doc = container.GetInstance<PhotoDocumentsService>()
                                        .GetById(photo.UserId + "_" + photo.Id);
                            }
                            GlobalQueue.Send(new AddPhotoLike
                            {
                                Id = subjectId.ToString(CultureInfo.InvariantCulture),
                                SubjectId = subjectId,
                                StartDate = doc.Created,
                                EndDate = DateTime.UtcNow,
                                PhotoId = photo.Id,
                                Image = doc.Image,
                                ImageBig = doc.ImageBig
                            });
                        }
                    }
                }
            }
        }
    }

    internal interface IConsumer
    {
        PhotoMessage Pull();
    }

    internal class PhotoMessage
    {
        public int UserId { get; set; }
        public int Id { get; set; }
    }
}
