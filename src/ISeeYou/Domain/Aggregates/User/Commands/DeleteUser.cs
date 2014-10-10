using ISeeYou.Platform.Domain.Messages;

namespace ISeeYou.Domain.Aggregates.User.Commands
{
    public class DeleteUser: Command
    {
        public string DeletedByUserId { get; set; }
    }
}