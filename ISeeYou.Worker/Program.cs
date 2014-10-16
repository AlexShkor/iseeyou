using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISeeYou.Domain.Aggregates.Subject.Commands;
using ISeeYou.Vk.Api;
using StructureMap;

namespace ISeeYou.Worker
{
    class Program
    {
        private static IEnumerable<int> _subjects;

        static void Main(string[] args)
        {
            var container = ObjectFactory.Container;
            new Bootstrapper().ConfigureSettings(container);
            new Bootstrapper().ConfigureMongoDb(container);
            var consumer = container.GetInstance<IConsumer>();
            while (true)
            {
                var photo = consumer.Pull();
                var api = new VkApi();
                var result = api.Likes(photo.Id, photo.UserId);
                if (result != null && result.Any())
                {
                    var intersect = result.Intersect(_subjects);
                    foreach (var subjectId in intersect)
                    {
                        Console.WriteLine("Found for {0}!!!", subjectId);
                        GlobalQueue.Send(new AddPhotoLike
                        {
                            Id = subjectId.ToString(CultureInfo.InvariantCulture),
                            SubjectId = subjectId,
                            StartDate = photo.Created,
                            EndDate = DateTime.UtcNow,
                            PhotoId = photo.Id,
                            //Image = photoDto.src,
                          //  ImageBig = photoDto.src_big
                        });
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
        public DateTime Created { get; set; }
    }
}
