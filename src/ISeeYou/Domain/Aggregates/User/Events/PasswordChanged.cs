using ISeeYou.Platform.Domain.Messages;

namespace ISeeYou.Domain.Aggregates.User.Events
{
    public class PasswordChanged : Event
    {
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public bool WasChangedByAdmin { get; set; }
    }
}