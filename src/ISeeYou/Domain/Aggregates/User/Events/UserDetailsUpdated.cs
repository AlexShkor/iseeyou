using ISeeYou.Platform.Domain.Messages;

namespace ISeeYou.Domain.Aggregates.User.Events
{
    public class UserDetailsUpdated: Event
    {
        public string UserName { get; set; }
    }
}