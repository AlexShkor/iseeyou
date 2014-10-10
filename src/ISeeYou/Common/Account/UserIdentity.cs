using ISeeYou.Views;

namespace ISeeYou.Common.Account
{
    public class UserIdentity
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }


        public UserIdentity(UserView view)
        {
            UserId = view.Id;
            UserName = view.UserName;
            Email = view.Email;
        }
    }
}