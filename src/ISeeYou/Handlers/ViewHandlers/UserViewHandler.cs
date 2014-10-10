using ISeeYou.Databases;
using ISeeYou.Domain.Aggregates.User.Events;
using ISeeYou.Platform.Dispatching;
using ISeeYou.Platform.Dispatching.Attributes;
using ISeeYou.Platform.Dispatching.Interfaces;
using ISeeYou.Views;
using Uniform;

namespace ISeeYou.Handlers.ViewHandlers
{
    [Priority(PriorityStages.ViewHandling)]
    public class UserViewHandler : IMessageHandler
    {
        private readonly ViewDatabase _db;
        private readonly IDocumentCollection<UserView> _users;

        public UserViewHandler(ViewDatabase db)
        {
            _db = db;
            _users = db.Users;
        }

        public void Handle(UserCreated e)
        {
            CreateUser(e);
        }

        public void Handle(UserReCreated e)
        {
            CreateUser(e);
        }

        private void CreateUser(UserCreated e)
        {
            var user = new UserView
            {
                Id = e.Id,
                UserName = e.UserName,
                FacebookId = e.FacebookId,
                Email = e.Email.ToLowerInvariant(),
                PasswordHash = e.PasswordHash,
                PasswordSalt = e.PasswordSalt,
                CreationDate = e.CreationDate,
                Cash = e.Cash
            };
            _users.Save(user);
        }

        public void Handle(PasswordChanged e)
        {
            _users.Update(e.Id, u =>
            {
                u.PasswordHash = e.PasswordHash;
                u.PasswordSalt = e.PasswordSalt;
            });
        }

        public void Handle(ProfileAvatarSet e)
        {
            _users.Update(e.Id, u =>
            {
                u.AvatarId = e.AvatarId;
            });
        }

        public void Handle(UserDeleted e)
        {
            _users.Delete(e.Id);
        }

    }
}