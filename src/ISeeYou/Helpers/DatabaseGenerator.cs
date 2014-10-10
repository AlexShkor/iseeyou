using ISeeYou.Domain.Aggregates.Site.Commands;
using ISeeYou.Domain.Aggregates.Site.Data;
using ISeeYou.Domain.Aggregates.User.Commands;
using ISeeYou.Platform.Domain;
using MongoDB.Bson;

namespace ISeeYou.Helpers
{

    public class DatabaseGenerator
    {
        public const string AdminUserName = "admin";
        private readonly ICommandBus _bus;

        public DatabaseGenerator(ICommandBus bus)
        {
            _bus = bus;
        }

        public void SetupInitialData()
        {
            CreateSite();
            CreateAdmin();
        }


        private static string NewId()
        {
            return ObjectId.GenerateNewId().ToString();
        }


        private void CreateSite()
        {
            var createSite = new CreateSite {Id = SiteSettings.SiteId };
            _bus.Send(createSite);
            _bus.Send(new SetSiteSettings
            {
                Id = createSite.Id,
                SmtpSettings = new SmtpSettingsData
                {
                    SmtpServer = "smtp.sendgrid.net",
                    SmtpServerPort = 587,
                    Email = "no-reply@paqk.com",
                    Username = "paqk",
                    Password = "1",
                }
            });
           
        }

        private void CreateAdmin()
        {
            var cmd = new CreateUser
            {
                UserName = AdminUserName,
                Email = AdminUserName,
                Password = "1",
            };

            _bus.Send(cmd);
        }
    }
}