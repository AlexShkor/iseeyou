using System.Security.Principal;

namespace ISeeYou.Authentication
{
    public class AkqIdentity : IIdentity
    {
        private readonly string _id;
        private readonly string _email;
        private readonly string _username;

        public AkqIdentity(string id, string email, string username)
        {
            _id = id;
            _email = email;
            _username = username;
        }

        public string Id
        {
            get { return _id; }
        }

        public string Name
        {
            get { return _username; }
        }

        public string Email
        {
            get { return _email; }
        }

        public string AuthenticationType
        {
            get { return "Forms"; }
        }

        public bool IsAuthenticated
        {
            get { return true; }
        }
    }
}