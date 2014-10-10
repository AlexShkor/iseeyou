using ISeeYou.Domain.Aggregates.User.Commands;
using ISeeYou.Helpers;
using ISeeYou.Platform.Dispatching.Interfaces;
using ISeeYou.Platform.Domain.Interfaces;

namespace ISeeYou.Domain.Aggregates.User
{
    public class UserApplicationService : IMessageHandler
    {
        private readonly IRepository<UserAggregate> _repository;
        private readonly CryptographicHelper _crypto;

        public UserApplicationService(IRepository<UserAggregate> repository,CryptographicHelper crypto)
        {
            _repository = repository;
            _crypto = crypto;
        }

        public void Handle(CreateUser c)
        {
            var salt = _crypto.GenerateSalt();
            var id = _crypto.GetMd5Hash(c.Email);
            _repository.Perform(id, user => user.Create(
                id,
                c.UserName,
                _crypto.GetPasswordHash(c.Password,salt),
                salt,
                c.Email,
                c.FacebookId));
        }

        public void Handle(ChangePassword c)
        {
            var salt = _crypto.GenerateSalt();
            var hash = _crypto.GetPasswordHash(c.NewPassword, salt);
            _repository.Perform(c.Id, user => user.ChangePassword(hash,salt,c.IsChangedByAdmin));
        }

        public void Handle(DeleteUser c)
        {
            _repository.Perform(c.Id, user => user.Delete(c));
        }

        public void Handle(UpdateUserDetails c)
        {
            _repository.Perform(c.Id, user => user.UpdateDetails(c));
        }

        public void Handle(SetProfileAvatar c)
        {
            _repository.Perform(c.Id, user => user.SetProfileAvatar(c.AvatarId));
        }
    }
}