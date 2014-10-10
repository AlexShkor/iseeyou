using ISeeYou.Platform.Domain.Messages;

namespace ISeeYou.Domain.Aggregates.User.Commands
{
    public class ChangePassword: Command
    {
        public string NewPassword { get; set; }
        public bool IsChangedByAdmin { get; set; }
    }
}