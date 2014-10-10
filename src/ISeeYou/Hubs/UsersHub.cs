using System.Threading.Tasks;
using ISeeYou.Authentication;
using Microsoft.AspNet.SignalR;

namespace ISeeYou.Hubs
{
    public class UsersHub: Hub
    {
        public static IHubContext CurrentContext
        {
            get
            {
                return GlobalHost.ConnectionManager.GetHubContext<UsersHub>();
            }
        }

        public void ConnectToTable(string tableId)
        {
            Groups.Add(Context.ConnectionId, tableId);
        }

        public override Task OnConnected()
        {
            return Task.Factory.StartNew(() =>
            {
                var userId = GetUserId();
                if (userId != null)
                    Groups.Add(Context.ConnectionId, userId);
            });
        }

        public override Task OnDisconnected()
        {
            return Task.Factory.StartNew(() =>
            {
                var userId = GetUserId();
                if (userId != null)
                    Groups.Remove(Context.ConnectionId, userId);
            });
        }


        private string GetUserId()
        {
            if (Context.User == null)
                return null;

            var identity = Context.User.Identity;
            if (identity.IsAuthenticated == false)
            {
                return null;
            }
            return ((AkqIdentity)identity).Id;
        }
    }
}