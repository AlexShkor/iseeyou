using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISeeYou.Databases;
using ISeeYou.Platform.Mongo;
using ISeeYou.Platform.Settings;
using ISeeYou.Platform.StructureMap;
using Microsoft.Practices.ServiceLocation;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Serializers;
using StructureMap;

namespace ISeeYou.VkRanking
{
    public class Bootstrapper
    {
        public void Configure(IContainer container, bool isReplayMode = false)
        {
            ConfigureSettings(container);
            ConfigureMongoDb(container);
            ServiceLocator.SetLocatorProvider(() => new StructureMapServiceLocator(container));
        }

        public void ConfigureSettings(IContainer container)
        {
            var settings = SettingsMapper.Map<SiteSettings>();
            container.Configure(config => config.For<SiteSettings>().Singleton().Use(settings));
        }

        public void ConfigureMongoDb(IContainer container)
        {
            // Register bson serializer conventions
            var myConventions = new ConventionPack
            {
                new NoDefaultPropertyIdConvention(),
                new IgnoreExtraElementsConvention(true),
            };
            ConventionRegistry.Register("MyConventions", myConventions, type => true);

            BsonSerializer.RegisterSerializer(typeof(DateTime), new DateTimeSerializer(DateTimeSerializationOptions.UtcInstance));
            BsonSerializer.RegisterSerializer(typeof(DateTime?), new NullableSerializer<DateTime>());

            var settings = container.GetInstance<SiteSettings>();
            container.Configure(config =>
            {
                config.For<MongoViewDatabase>().Singleton().Use(new MongoViewDatabase(settings.MongoViewConnectionString).EnsureIndexes());
                config.For<MongoLogsDatabase>().Singleton().Use(new MongoLogsDatabase(settings.MongoLogsConnectionString).EnsureIndexes());
                config.For<MongoEventsDatabase>().Singleton().Use(new MongoEventsDatabase(settings.MongoEventsConnectionString));
                config.For<MongoAdminDatabase>().Singleton().Use(new MongoAdminDatabase(settings.MongoAdminConnectionString));
            });
        }
    }
}
