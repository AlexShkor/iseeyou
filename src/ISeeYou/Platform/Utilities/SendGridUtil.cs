using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using SendGridMail;
using SendGridMail.Transport;

namespace ISeeYou.Platform.Utilities
{
    public class SendGridUtil
    {
        private readonly SiteSettings _settings;
        private const string From = "admin@edpm.com";

        public SendGridUtil(SiteSettings settings)
        {
            _settings = settings;
        }

        public void SendMessage(IEnumerable<string> to, string subject, string body)
        {
            var message = SendGrid.GenerateInstance();
            foreach (string recipient in to)
            {
                message.AddTo(recipient);
            }
            message.From = new MailAddress(From);
            message.Html = body;
            message.Subject = subject;
            var transportInstance = REST.GetInstance(new NetworkCredential(_settings.SendgridUsername, _settings.SendgridPassword));
            transportInstance.Deliver(message);
        }
    }
}