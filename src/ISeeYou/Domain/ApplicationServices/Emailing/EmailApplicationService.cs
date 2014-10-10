using ISeeYou.Domain.ApplicationServices.Emailing.Commands;
using ISeeYou.Platform.Dispatching.Interfaces;
using ISeeYou.Platform.Utilities;

namespace ISeeYou.Domain.ApplicationServices.Emailing
{
    public class EmailApplicationService: IMessageHandler
    {
        private readonly SendGridUtil _sendGrid;

        public EmailApplicationService(SendGridUtil sendGrid)
        {
            _sendGrid = sendGrid;
        }

        public void Handle(SendMail c)
        {
            _sendGrid.SendMessage(c.Recipients, c.Subject, c.Body);
        }
    }
}