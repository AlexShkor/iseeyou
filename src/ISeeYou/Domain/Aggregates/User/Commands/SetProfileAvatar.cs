using ISeeYou.Platform.Domain.Messages;

namespace ISeeYou.Domain.Aggregates.User.Commands
{
    public class SetProfileAvatar: Command
    {
        public string AvatarId { get; set; }
    }
}