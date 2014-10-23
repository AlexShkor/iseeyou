using System;
using System.Globalization;
using System.Linq;
using ISeeYou.MQ;
using ISeeYou.MQ.Events;
using ISeeYou.ViewServices;
using ISeeYou.Vk.Api;
using StructureMap;

namespace ISeeYou.Workers
{
    public class SourceWorker
    {
        public static void Start()
        {
            var container = ObjectFactory.Container;
            new Bootstrapper().ConfigureSettings(container);
            new Bootstrapper().ConfigureMongoDb(container);
            var api = new VkApi();

            var settings = container.GetInstance<SiteSettings>();
            const string type = "photo";
            var consumer = new RabbitMqConsumer<SourceFetchEvent>(settings.RabbitHost, settings.RabbitUser, settings.RabbitPwd, settings.SourcesQueue);
            var photosService = container.GetInstance<PhotoDocumentsService>();
            consumer.On += sourceFetchEvent =>
            {
                var source = sourceFetchEvent.Payload;
                Console.WriteLine("Source processing started. User ID: {0}", source.UserId);
                try
                {
                    var albums =
                        api.GetAlbums(source.UserId)
                            .Select(x => x.aid.ToString(CultureInfo.InvariantCulture))
                            .Concat(new[] { "profile", "wall" });
                    foreach (var album in albums)
                    {
                        var photos =
                            api.GetPhotos(source.UserId, album)
                                .Where(x => x.likes.count > 0);
                        var exist = photosService.GetPhotoIdsFor(source.UserId).ToList();
                        try
                        {
                            var newPhotos = photos.Where(x => !exist.Contains(x.pid)).OrderByDescending(x=> x.created);
                            var counter = 0;
                            foreach (var newPhoto in newPhotos)
                            {
                                var id = source.UserId + "_" + type + newPhoto.pid;
                                photosService.InsertAsync(new PhotoDocument
                                {
                                    Added = DateTime.UtcNow,
                                    New = source.New,
                                    Id = id,
                                    Created = UnixTimeStampToDateTime(newPhoto.created),
                                    PhotoId = newPhoto.pid,
                                    SourceId = source.UserId,
                                    Image = newPhoto.src,
                                    ImageBig = newPhoto.src_big,
                                    NextFetching = DateTime.UtcNow + TimeSpan.FromSeconds((counter > 100 ? counter : 0)*2)
                                });
                                counter++;
                            }
                        }
                        catch
                        {
                            
                        }
                    }
                }
                catch (Exception e)
                {

                }
            };
            consumer.Start();
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