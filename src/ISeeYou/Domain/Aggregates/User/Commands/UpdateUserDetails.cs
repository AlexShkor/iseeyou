using ISeeYou.Platform.Domain.Messages;

namespace ISeeYou.Domain.Aggregates.User.Commands
{
    public class UpdateUserDetails: Command
    {
        public string UserName { get; set; }
        public string Id { get; set; }
    }
}