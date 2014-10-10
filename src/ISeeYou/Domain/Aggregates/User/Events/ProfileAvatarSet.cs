using ISeeYou.Platform.Domain.Messages;

namespace ISeeYou.Domain.Aggregates.User.Events
{
    public class ProfileAvatarSet: Event
    {
        public string AvatarId { get; set; }
    }
}