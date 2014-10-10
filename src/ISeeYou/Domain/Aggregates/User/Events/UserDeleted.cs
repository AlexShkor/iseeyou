using ISeeYou.Platform.Domain.Messages;

namespace ISeeYou.Domain.Aggregates.User.Events
{
    public class UserDeleted: Event
    {
        public string DeletedByUserId { get; set; }
    }
}